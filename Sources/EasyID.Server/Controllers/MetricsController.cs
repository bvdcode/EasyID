using EasyID.Server.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyID.Server.Controllers
{
    public class MetricsController(AppDbContext _dbContext) : ControllerBase
    {
        [HttpGet(Routes.Metrics + "/has-users")]
        public Task<bool> HasUsers()
        {
            return _dbContext.Users.AnyAsync();
        }
    }
}
