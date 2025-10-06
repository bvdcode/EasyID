using Mapster;
using EasyExtensions;
using EasyID.Server.Database;
using Microsoft.AspNetCore.Mvc;
using EasyID.Server.Extensions;
using EasyID.Server.Models.Dto;
using EasyExtensions.Abstractions;
using EasyID.Server.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EasyID.Server.Controllers
{
    public class UsersController(AppDbContext _dbContext, IPasswordHashService _hashService) : ControllerBase
    {
        [Authorize]
        [HttpGet(Routes.Users + "/{id:guid}")]
        public async Task<UserDto> GetUser(Guid id)
        {
            User user = await _dbContext.GetUserAsync(id);
            var dto = user.Adapt<UserDto>();

            // Load related data: groups -> roles -> permissions
            var groups = await _dbContext.GroupUsers
                .Where(gu => gu.UserId == user.Id)
                .Select(gu => gu.Group)
                .ToListAsync();
            dto.Groups = [.. groups.Select(g => g.Name)
                .Distinct()
                .OrderBy(s => s)];

            var roleIds = await _dbContext.RoleGroups
                .Where(rg => groups
                .Select(g => g.Id)
                .Contains(rg.GroupId))
                .Select(rg => rg.RoleId)
                .Distinct()
                .ToListAsync();

            var roles = await _dbContext.Roles
                .Where(r => roleIds.Contains(r.Id))
                .ToListAsync();

            dto.Roles = [.. roles
                .Select(r => r.Name)
                .Distinct()
                .OrderBy(s => s)];

            var permissionIds = await _dbContext.PermissionRoles
                .Where(pr => roleIds.Contains(pr.RoleId))
                .Select(pr => pr.PermissionId)
                .Distinct()
                .ToListAsync();

            var permissions = await _dbContext.Permissions
                .Where(p => permissionIds.Contains(p.Id))
                .ToListAsync();

            dto.Permissions = [.. permissions
                .Select(p => p.Name)
                .Distinct()
                .OrderBy(s => s)];

            return dto;
        }

        [Authorize]
        [HttpPost(Routes.Users + "/{id:guid}/password")]
        public async Task<IActionResult> ChangePassword([FromRoute] Guid id, [FromBody] UpdateUserPasswordRequestDto request)
        {
            User user = await _dbContext.GetUserAsync(id);
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
            User user = await _dbContext.GetUserAsync(id);
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
