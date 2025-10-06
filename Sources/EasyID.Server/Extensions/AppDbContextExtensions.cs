using EasyExtensions;
using EasyID.Server.Database;
using System.Security.Claims;
using EasyID.Server.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyID.Server.Extensions
{
    public static class AppDbContextExtensions
    {
        public static async Task<User> GetUserAsync(this AppDbContext dbContext, ClaimsPrincipal claims)
        {
            Guid userId = claims.GetUserId();
            User? found = await dbContext.Users.FindAsync(userId);
            return found ?? throw new UnauthorizedAccessException(userId.ToString());
        }

        public static async Task<User> GetUserAsync(this AppDbContext dbContext, Guid userId)
        {
            return await dbContext.Users.FirstAsync(u => u.Id == userId);
        }
    }
}
