using Microsoft.EntityFrameworkCore;
using EasyID.Server.Database.Models;
using EasyExtensions.EntityFrameworkCore.Database;

namespace EasyID.Server.Database
{
    public class AppDbContext(DbContextOptions options) : AuditedDbContext(options)
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<RoleGroup> RoleGroups { get; set; } = null!;
        public DbSet<GroupUser> GroupUsers { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<PermissionRole> PermissionRoles { get; set; } = null!;
        public DbSet<LoginAuditEvent> LoginAuditEvents { get; set; } = null!;
    }
}