using Mapster;
using SixLabors.ImageSharp;
using EasyID.Server.Database;
using EasyID.Server.Extensions;
using EasyID.Server.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using EasyExtensions.Abstractions;
using EasyID.Server.Database.Models;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.AspNetCore.Authorization;

namespace EasyID.Server.Controllers
{
    public class UsersController(AppDbContext _dbContext, IConfiguration _configuration,
        IPasswordHashService _hashService) : ControllerBase
    {
        [Authorize]
        [HttpGet(Routes.Users + "/me")]
        public async Task<UserDto> Me()
        {
            User user = await _dbContext.GetUserAsync(User);
            return user.Adapt<UserDto>();
        }

        [HttpGet(Routes.Users + "/{id}/avatar")]
        [HttpGet(Routes.Users + "/{id}/avatar.webp")]
        public async Task<IActionResult> GetAvatar([FromRoute] Guid id)
        {
            User? user = await _dbContext.Users.FindAsync(id);
            if (user == null || user.AvatarWebPBytes == null)
            {
                return NotFound();
            }
            return File(user.AvatarWebPBytes, "image/webp");
        }

        [Authorize]
        [HttpPost(Routes.Users + "/me/password")]
        public async Task<IActionResult> ChangePassword([FromBody] UpdateUserPasswordRequestDto request)
        {
            User user = await _dbContext.GetUserAsync(User);
            if (!string.IsNullOrWhiteSpace(request.OldPassword) && !string.IsNullOrWhiteSpace(request.NewPassword))
            {
                if (!_hashService.Verify(request.OldPassword, user.PasswordPhc, out _))
                {
                    return BadRequest("Old password is incorrect.");
                }
                user.PasswordPhc = _hashService.Hash(request.NewPassword);
                user.PasswordVersion = _hashService.PasswordHashVersion;
            }
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [Authorize]
        [HttpPost(Routes.Users + "/me")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserRequestDto request)
        {
            User user = await _dbContext.GetUserAsync(User);
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.MiddleName = request.MiddleName;
            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                user.Username = request.Username;
            }
            if (!string.IsNullOrWhiteSpace(request.OldPassword) && !string.IsNullOrWhiteSpace(request.NewPassword))
            {
                if (!_hashService.Verify(request.OldPassword, user.PasswordPhc, out _))
                {
                    return BadRequest("Old password is incorrect.");
                }
                user.PasswordPhc = _hashService.Hash(request.NewPassword);
                user.PasswordVersion = _hashService.PasswordHashVersion;
            }
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [Authorize]
        [HttpPut(Routes.Users + "/me/avatar")]
        public async Task<IActionResult> UpdateAvatar([FromForm] IFormFile file)
        {
            User user = await _dbContext.GetUserAsync(User);
            if (file.Length > 2 * 1024 * 1024)
            {
                return BadRequest("Avatar file size exceeds the limit of 2MB.");
            }
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = default;
            using var loadedImage = await Image.LoadAsync<Rgba32>(memoryStream);
            int avatarSizeLimit = _configuration.GetValue("AvatarSizeLimit", 1024);
            loadedImage.Mutate(x => x.Resize(avatarSizeLimit, 0));
            using var outputStream = new MemoryStream();
            await loadedImage.SaveAsWebpAsync(outputStream);
            user.AvatarWebPBytes = outputStream.ToArray();
            await _dbContext.SaveChangesAsync();
            return Created($"{Routes.Users}/{user.Id}/avatar", null);
        }
    }
}
