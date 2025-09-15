using EasyID.Server.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyID.Server.Controllers
{
    public class MetricsController(AppDbContext _dbContext) : ControllerBase
    {
        [HttpGet(Routes.Metrics)]
        public async Task<IActionResult> GetServerMetrics()
        {
            bool hasUsers = await _dbContext.Users.AnyAsync();
            return Ok(new
            {
                hasUsers,
                serverTime = DateTime.Now
            });
        }
    }
}
