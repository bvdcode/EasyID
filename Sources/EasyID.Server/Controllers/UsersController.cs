using Mapster;
using EasyID.Server.Database;
using EasyID.Server.Extensions;
using EasyID.Server.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EasyID.Server.Controllers
{
    public class UsersController(AppDbContext _dbContext) : ControllerBase
    {
        [Authorize]
        [HttpGet(Routes.Users + "/me")]
        public async Task<UserDto> Me()
        {
            User user = await _dbContext.GetUserAsync(User);
            return user.Adapt<UserDto>();
        }
    }
}
