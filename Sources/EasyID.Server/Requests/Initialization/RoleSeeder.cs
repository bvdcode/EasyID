using EasyID.Server.Database;
using EasyID.Server.Database.Models;

namespace EasyID.Server.Requests.Initialization
{
    internal static class RoleSeeder
    {
        public static async Task<IReadOnlyList<Role>> SeedSystemRolesAsync(
            AppDbContext dbContext,
            CancellationToken cancellationToken)
        {
            var adminRole = new Role
            {
                IsSystem = true,
                Name = Constants.SystemRoles.Admin,
                DisplayName = Constants.AppName + " Administrator",
                Description = "System administrator role with full access to administration features.",
            };

            var userRole = new Role
            {
                IsSystem = true,
                Name = Constants.SystemRoles.User,
                DisplayName = Constants.AppName + " User",
                Description = "Default user role with limited permissions.",
            };

            await dbContext.Roles.AddRangeAsync([adminRole, userRole], cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return [adminRole, userRole];
        }

        public static async Task LinkRolesToGroupsAsync(
            AppDbContext dbContext,
            IEnumerable<Role> roles,
            IEnumerable<Group> groups,
            CancellationToken cancellationToken)
        {
            var adminRole = roles.First(r => r.Name == Constants.SystemRoles.Admin);
            var userRole = roles.First(r => r.Name == Constants.SystemRoles.User);
            var adminGroup = groups.First(g => g.Name == Constants.SystemGroups.Admins);
            var usersGroup = groups.First(g => g.Name == Constants.SystemGroups.Users);

            await dbContext.RoleGroups.AddRangeAsync(
            [
                new RoleGroup { RoleId = adminRole.Id, GroupId = adminGroup.Id },
                new RoleGroup { RoleId = userRole.Id, GroupId = usersGroup.Id }
            ], cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public static async Task GrantPermissionsToRolesAsync(
            AppDbContext dbContext,
            IEnumerable<Permission> permissions,
            IEnumerable<Role> roles,
            CancellationToken cancellationToken)
        {
            var adminRole = roles.First(r => r.Name == Constants.SystemRoles.Admin);
            var userRole = roles.First(r => r.Name == Constants.SystemRoles.User);

            await GrantAllPermissionsToAdminAsync(dbContext, permissions, adminRole, cancellationToken);
            await GrantBasicPermissionsToUserAsync(dbContext, permissions, userRole, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private static async Task GrantAllPermissionsToAdminAsync(
            AppDbContext dbContext,
            IEnumerable<Permission> permissions,
            Role adminRole,
            CancellationToken cancellationToken)
        {
            var adminPermissions = permissions
                .Select(p => new PermissionRole
                {
                    RoleId = adminRole.Id,
                    PermissionId = p.Id
                });

            await dbContext.PermissionRoles.AddRangeAsync(adminPermissions, cancellationToken);
        }

        private static async Task GrantBasicPermissionsToUserAsync(
            AppDbContext dbContext,
            IEnumerable<Permission> permissions,
            Role userRole,
            CancellationToken cancellationToken)
        {
            var permissionsList = permissions.ToList();
            var basicPermissions = new[]
            {
                Constants.SystemPermissions.Users.View,
                Constants.SystemPermissions.Users.ChangeAvatar,
                Constants.SystemPermissions.Users.ChangePassword
            };

            var userPermissions = permissionsList
                .Where(p => basicPermissions.Contains(p.Name))
                .Select(p => new PermissionRole
                {
                    RoleId = userRole.Id,
                    PermissionId = p.Id
                });

            await dbContext.PermissionRoles.AddRangeAsync(userPermissions, cancellationToken);
        }
    }
}
