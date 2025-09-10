using EasyID.Server.Database;
using System.Security.Claims;
using EasyExtensions.Helpers;
using EasyID.Server.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using EasyExtensions.AspNetCore.Authorization.Services;

namespace EasyID.Server.Controllers
{
    [ApiController]
    public class AuthController(ILogger<AuthController> _logger, AppDbContext _dbContext,
        ITokenProvider _tokenProvider) : ControllerBase
    {
        [HttpPost(Routes.Auth + "/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
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