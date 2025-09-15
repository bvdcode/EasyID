using Microsoft.EntityFrameworkCore;
using EasyID.Server.Database.Models;
using EasyExtensions.EntityFrameworkCore.Database;

namespace EasyID.Server.Database
{
    public class AppDbContext(DbContextOptions options) : AuditedDbContext(options)
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<GroupUser> GroupUsers { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    }
}