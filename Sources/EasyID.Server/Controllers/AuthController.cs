using MediatR;
using System.Net;
using EasyExtensions.Helpers;
using EasyID.Server.Database;
using EasyID.Server.Requests;
using EasyID.Server.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using EasyExtensions.Abstractions;
using EasyID.Server.Database.Models;
using Microsoft.EntityFrameworkCore;
using EasyExtensions.AspNetCore.Extensions;
using EasyExtensions.EntityFrameworkCore.Exceptions;
using EasyExtensions.AspNetCore.Authorization.Services;

namespace EasyID.Server.Controllers
{
    [ApiController]
    public class AuthController(ILogger<AuthController> _logger, AppDbContext _dbContext,
        ITokenProvider _tokenProvider, IPasswordHashService _hashService, IMediator _mediator) : ControllerBase
    {
        [HttpDelete(Routes.Auth + "/logout")]
        public async Task<IActionResult> Logout([FromQuery] string refreshToken)
        {
            var foundToken = _dbContext.RefreshTokens
                .Where(rt => rt.Token == refreshToken)
                .FirstOrDefault();
            if (foundToken != null)
            {
                _dbContext.RefreshTokens.Remove(foundToken);
                await _dbContext.SaveChangesAsync();
            }
            return NoContent();
        }

        [HttpPost(Routes.Auth + "/refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto request)
        {
            var foundToken = _dbContext.RefreshTokens
                .Where(rt => rt.Token == request.RefreshToken)
                .FirstOrDefault() ?? throw new EntityNotFoundException(nameof(RefreshToken));
            foundToken.Token = StringHelpers.CreatePseudoRandomString(64);
            foundToken.IpAddress = IPAddress.Parse(Request.GetRemoteAddress());
            foundToken.UserAgent = Request.Headers.UserAgent.ToString();
            await _dbContext.SaveChangesAsync();

            string accessToken = _tokenProvider.CreateToken(x => x.AddRange(foundToken.User.GetClaims().Claims));
            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = foundToken.Token,
            });
        }

        [HttpPost(Routes.Auth + "/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            bool hasUsers = await _dbContext.Users.AnyAsync();
            if (!hasUsers)
            {
                InitializeInstanceQuery initQuery = new()
                {
                    FirstLoginRequest = request
                };
                await _mediator.Send(initQuery);
            }

            string requestUsername = request.Username.ToLower().Trim();
#pragma warning disable CA1862 // Use the 'StringComparison' method overloads to perform case-insensitive string comparisons
            var foundUser = await _dbContext.Users
                .Where(u => u.Username.ToLower() == requestUsername || u.Email.ToLower() == requestUsername)
                .FirstOrDefaultAsync();
#pragma warning restore CA1862 // Use the 'StringComparison' method overloads to perform case-insensitive string comparisons
            if (foundUser == null || string.IsNullOrWhiteSpace(foundUser.PasswordPhc))
            {
                _logger.LogWarning("Login attempt with unknown user {Username}", request.Username);
                return Unauthorized(new { Error = "Invalid username or password" });
            }

            bool isValid = _hashService.Verify(request.Password, foundUser.PasswordPhc, out bool needsUpgrade);
            if (!isValid)
            {
                _logger.LogWarning("Login attempt with invalid password for user {Username}", request.Username);
                return Unauthorized(new { Error = "Invalid username or password" });
            }
            if (needsUpgrade)
            {
                foundUser.PasswordPhc = _hashService.Hash(request.Password);
                _dbContext.Users.Update(foundUser);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Upgraded password hash for user {Username}", request.Username);
            }

            string accessToken = _tokenProvider.CreateToken(x => x.AddRange(foundUser.GetClaims().Claims));
            RefreshToken refreshToken = new()
            {
                City = "local",
                Country = "local",
                UserId = foundUser.Id,
                UserAgent = Request.Headers.UserAgent.ToString(),
                Token = StringHelpers.CreatePseudoRandomString(64),
                IpAddress = IPAddress.Parse(Request.GetRemoteAddress()),
            };
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();
            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
            });
        }
    }
}