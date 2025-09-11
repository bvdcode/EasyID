using EasyID.Server.Database;
using System.Security.Claims;
using EasyExtensions.Helpers;
using EasyID.Server.Requests;
using EasyExtensions.Services;
using EasyID.Server.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyExtensions.AspNetCore.Authorization.Services;
using MediatR;

namespace EasyID.Server.Controllers
{
    [ApiController]
    public class AuthController(ILogger<AuthController> _logger, AppDbContext _dbContext,
        ITokenProvider _tokenProvider, IConfiguration _configuration, IMediator _mediator) : ControllerBase
    {
        private readonly string _pepper = _configuration.GetValue<string>("Encryption:Pepper")
            ?? throw new ArgumentNullException("Encryption:Pepper", "Encryption key cannot be null.");

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

            var foundUser = await _dbContext.Users
                .Where(u => u.Username == request.Username || u.Email == request.Username)
                .FirstOrDefaultAsync();
            if (foundUser == null || string.IsNullOrWhiteSpace(foundUser.PasswordPhc))
            {
                _logger.LogWarning("Login attempt with unknown user {Username}", request.Username);
                return Unauthorized(new { Error = "Invalid username or password" });
            }

            Pbkdf2PasswordHashService pbk = new(_pepper);
            bool isValid = pbk.Verify(request.Password, foundUser.PasswordPhc, out bool needsUpgrade);
            if (!isValid)
            {
                _logger.LogWarning("Login attempt with invalid password for user {Username}", request.Username);
                return Unauthorized(new { Error = "Invalid username or password" });
            }

            string accessToken = _tokenProvider.CreateToken(x => x.Add(new Claim("sub", "test")));
            string refreshToken = StringHelpers.CreatePseudoRandomString(16);
            return Ok(new
            {
                ExpiresIn = 3600,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            });
        }
    }
}