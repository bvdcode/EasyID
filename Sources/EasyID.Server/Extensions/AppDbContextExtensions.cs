using EasyExtensions;
using EasyID.Server.Database;
using System.Security.Claims;
using EasyID.Server.Database.Models;
using Microsoft.EntityFrameworkCore;
using EasyExtensions.EntityFrameworkCore.Exceptions;

namespace EasyID.Server.Extensions
{
    public static class AppDbContextExtensions
    {
        public static async Task<User> GetUserAsync(this AppDbContext dbContext, ClaimsPrincipal claims)
        {
            Guid userId = claims.GetUserId();
            User? found = await dbContext.Users.FindAsync(userId);
            return found ?? throw new EntityNotFoundException(nameof(User));
        }

        public static async Task<User> GetUserAsync(this AppDbContext dbContext, Guid userId)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId)
                ?? throw new EntityNotFoundException(nameof(User));
        }
    }
}
