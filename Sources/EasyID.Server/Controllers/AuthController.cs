using MediatR;
using System.Net;
using EasyID.Server.Database;
using EasyExtensions.Helpers;
using EasyID.Server.Requests;
using EasyExtensions.Services;
using EasyID.Server.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyID.Server.Database.Models;
using EasyExtensions.AspNetCore.Extensions;
using EasyExtensions.AspNetCore.Authorization.Services;

namespace EasyID.Server.Controllers
{
    [ApiController]
    public class AuthController(ILogger<AuthController> _logger, AppDbContext _dbContext,
        ITokenProvider _tokenProvider, Pbkdf2PasswordHashService _hashService, IMediator _mediator) : ControllerBase
    {
        [HttpPost(Routes.Auth + "/refresh")]
        public IActionResult Refresh([FromBody] RefreshRequestDto request)
        {
            return Ok();
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