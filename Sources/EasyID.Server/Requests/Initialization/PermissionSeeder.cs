using EasyID.Server.Database;
using EasyID.Server.Database.Models;

namespace EasyID.Server.Requests.Initialization
{
    internal static class PermissionSeeder
    {
        public static async Task<IReadOnlyList<Permission>> SeedSystemPermissionsAsync(
            AppDbContext dbContext,
            CancellationToken cancellationToken)
        {
            var permissions = new List<Permission>();

            AddUserPermissions(permissions);
            AddAppPermissions(permissions);
            AddFlagPermissions(permissions);
            AddGroupPermissions(permissions);
            AddPermissionManagementPermissions(permissions);

            await dbContext.Permissions.AddRangeAsync(permissions, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return permissions;
        }

        private static void AddUserPermissions(List<Permission> permissions)
        {
            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Users.View,
                DisplayName = "View Users",
                Description = "Allows viewing user details.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Users.Create,
                DisplayName = "Create Users",
                Description = "Allows creating new users.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Users.Edit,
                DisplayName = "Edit Users",
                Description = "Allows editing user details.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Users.Delete,
                DisplayName = "Delete Users",
                Description = "Allows deleting users.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Users.ChangeAvatar,
                DisplayName = "Change User Avatar",
                Description = "Allows changing user avatars.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Users.ChangePassword,
                DisplayName = "Change User Password",
                Description = "Allows changing user passwords.",
                IsSystem = true,
            });
        }

        private static void AddAppPermissions(List<Permission> permissions)
        {
            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Apps.View,
                DisplayName = "View Applications",
                Description = "Allows viewing registered applications.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Apps.Create,
                DisplayName = "Create Applications",
                Description = "Allows creating new applications.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Apps.Update,
                DisplayName = "Update Applications",
                Description = "Allows updating application settings.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Apps.Delete,
                DisplayName = "Delete Applications",
                Description = "Allows deleting applications.",
                IsSystem = true,
            });
        }

        private static void AddFlagPermissions(List<Permission> permissions)
        {
            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Flags.View,
                DisplayName = "View Flags",
                Description = "Allows viewing feature flags.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Flags.Create,
                DisplayName = "Create Flags",
                Description = "Allows creating new feature flags.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Flags.Update,
                DisplayName = "Update Flags",
                Description = "Allows updating feature flags.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Flags.Delete,
                DisplayName = "Delete Flags",
                Description = "Allows deleting feature flags.",
                IsSystem = true,
            });
        }

        private static void AddGroupPermissions(List<Permission> permissions)
        {
            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Groups.View,
                DisplayName = "View Groups",
                Description = "Allows viewing groups.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Groups.Create,
                DisplayName = "Create Groups",
                Description = "Allows creating new groups.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Groups.Update,
                DisplayName = "Update Groups",
                Description = "Allows updating group details.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Groups.Delete,
                DisplayName = "Delete Groups",
                Description = "Allows deleting groups.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Groups.ManageMembers,
                DisplayName = "Manage Group Members",
                Description = "Allows adding and removing group members.",
                IsSystem = true,
            });
        }

        private static void AddPermissionManagementPermissions(List<Permission> permissions)
        {
            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Permissions.View,
                DisplayName = "View Permissions",
                Description = "Allows viewing permissions.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Permissions.Create,
                DisplayName = "Create Permissions",
                Description = "Allows creating new permissions.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Permissions.Update,
                DisplayName = "Update Permissions",
                Description = "Allows updating permissions.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Permissions.Delete,
                DisplayName = "Delete Permissions",
                Description = "Allows deleting permissions.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Permissions.Assign,
                DisplayName = "Assign Permissions",
                Description = "Allows assigning permissions to roles.",
                IsSystem = true,
            });
        }
    }
}
