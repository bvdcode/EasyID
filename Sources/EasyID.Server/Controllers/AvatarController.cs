using SixLabors.ImageSharp;
using EasyID.Server.Database;
using Microsoft.AspNetCore.Mvc;
using EasyID.Server.Extensions;
using EasyID.Server.Database.Models;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.AspNetCore.Authorization;

namespace EasyID.Server.Controllers
{
    public class AvatarController(AppDbContext _dbContext, IConfiguration _configuration) : ControllerBase
    {
        [HttpGet(Routes.Users + "/{id:guid}/avatar")]
        [HttpGet(Routes.Users + "/{id:guid}/avatar.webp")]
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
        [HttpPut(Routes.Users + "/{id:guid}/avatar")]
        public async Task<IActionResult> UpdateAvatar([FromForm] IFormFile file)
        {
            User user = await _dbContext.GetUserAsync(User);
            int avatarFileSizeLimitMb = _configuration.GetValue("AvatarFileSizeLimitMB", 2);
            if (file.Length > avatarFileSizeLimitMb * 1024 * 1024)
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
