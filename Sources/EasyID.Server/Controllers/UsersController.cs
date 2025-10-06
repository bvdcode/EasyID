using Mapster;
using EasyExtensions;
using EasyID.Server.Database;
using Microsoft.AspNetCore.Mvc;
using EasyID.Server.Extensions;
using EasyID.Server.Models.Dto;
using EasyExtensions.Abstractions;
using EasyID.Server.Database.Models;
using Microsoft.AspNetCore.Authorization;

namespace EasyID.Server.Controllers
{
    public class UsersController(AppDbContext _dbContext, IPasswordHashService _hashService) : ControllerBase
    {
        [Authorize]
        [HttpGet(Routes.Users + "/me")]
        public Task<UserDto> Me() => GetUser(User.GetUserId());

        [Authorize]
        [HttpGet(Routes.Users + "/{id:guid}")]
        public async Task<UserDto> GetUser(Guid id)
        {
            User user = await _dbContext.GetUserAsync(id);
            return user.Adapt<UserDto>();
        }

        [Authorize]
        [HttpPost(Routes.Users + "/{id:guid}/password")]
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
        [HttpPatch(Routes.Users + "/{id:guid}")]
        public async Task<IActionResult> UpdateProfile([FromRoute] Guid id, [FromBody] UpdateUserRequestDto request)
        {
            User user = await _dbContext.GetUserAsync(User);
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.MiddleName = request.MiddleName;
            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                user.Username = request.Username;
            }
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
