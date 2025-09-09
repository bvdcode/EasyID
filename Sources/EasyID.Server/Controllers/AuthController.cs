using EasyID.Server.Database;
using Microsoft.AspNetCore.Mvc;

namespace EasyID.Server.Controllers
{
    [ApiController]
    public class AuthController(ILogger<AuthController> _logger, AppDbContext _dbContext) : ControllerBase
    {
        [HttpGet("/api/v1")]
        public async Task<IActionResult> OK()
        {
            return Ok();
        }
    }
}