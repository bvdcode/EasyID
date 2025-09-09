using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EasyID.Server.Controllers
{
    public class UsersController : ControllerBase
    {
        [Authorize]
        [HttpGet(Routes.Users + "/me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                Name = User.Identity?.Name,
            });
        }
    }
}
