using MediatR;
using EasyID.Server.Database;
using EasyID.Server.Models.Dto;
using EasyExtensions.Abstractions;
using EasyID.Server.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyID.Server.Requests
{
    public class InitializeInstanceQuery : IRequest
    {
        public LoginRequestDto FirstLoginRequest { get; set; } = null!;
    }

    public class IInitializeInstanceQueryHandler(IPasswordHashService hashService, AppDbContext dbContext) : IRequestHandler<InitializeInstanceQuery>
    {
        public async Task Handle(InitializeInstanceQuery request, CancellationToken cancellationToken)
        {
            if (await dbContext.Users.AnyAsync(cancellationToken))
            {
                throw new InvalidOperationException("The instance has already been initialized.");
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(request.FirstLoginRequest.Username, nameof(request.FirstLoginRequest.Username));
            ArgumentException.ThrowIfNullOrWhiteSpace(request.FirstLoginRequest.Password, nameof(request.FirstLoginRequest.Password));

            string hashedPassword = hashService.Hash(request.FirstLoginRequest.Password);
            bool usernameIsEmail = request.FirstLoginRequest.Username.Contains('@');
            string email = usernameIsEmail ? request.FirstLoginRequest.Username : "admin@localhost";
            string username = usernameIsEmail ? "admin" : request.FirstLoginRequest.Username;

            var user = new User
            {
                Email = email,
                FailedCount = 0,
                Username = username,
                FirstName = "Admin",
                PasswordPhc = hashedPassword,
                PasswordVersion = hashService.PasswordHashVersion,
            };
            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var groups = await CreateSystemGroupsAsync(user, cancellationToken);
            var permissions = await CreateSystemPermissionsAsync(cancellationToken);
            var roles = await CreateSystemRolesAsync(permissions, cancellationToken);
            await LinkRolesToGroupsAsync(roles, groups, cancellationToken);
            await GrantPermissionsToRolesAsync(permissions, roles, cancellationToken);
        }

        private async Task<IReadOnlyList<Group>> CreateSystemGroupsAsync(User user, CancellationToken ct)
        {
            var adminGroup = new Group
            {
                IsSystem = true,
                Name = Constants.SystemGroups.Admin,
                DisplayName = Constants.AppName + " Administrators",
                Description = "System administrators group with full access to administration features.",
            };
            var usersGroup = new Group
            {
                IsSystem = true,
                Name = Constants.SystemGroups.Users,
                DisplayName = Constants.AppName + " Users",
                Description = "Default users group with limited permissions.",
            };
            await dbContext.Groups.AddRangeAsync([adminGroup, usersGroup], ct);
            await dbContext.SaveChangesAsync(ct);

            // Add the initial user into both groups.
            await dbContext.GroupUsers.AddRangeAsync(
            [
                new GroupUser { GroupId = adminGroup.Id, UserId = user.Id },
                new GroupUser { GroupId = usersGroup.Id, UserId = user.Id }
            ], ct);
            await dbContext.SaveChangesAsync(ct);

            return [adminGroup, usersGroup];
        }

        private async Task<IReadOnlyList<Permission>> CreateSystemPermissionsAsync(CancellationToken ct)
        {
            var permissions = new List<Permission>();
            AddUserPermissions(permissions);
            await dbContext.Permissions.AddRangeAsync(permissions, ct);
            await dbContext.SaveChangesAsync(ct);
            return permissions;
        }

        private static void AddUserPermissions(List<Permission> permissions)
        {
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
            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Users.Create,
                DisplayName = "Create Users",
                Description = "Allows creating new users.",
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
                Name = Constants.SystemPermissions.Users.Edit,
                DisplayName = "Edit Users",
                Description = "Allows editing user details.",
                IsSystem = true,
            });
            permissions.Add(new Permission
            {
                Name = Constants.SystemPermissions.Users.View,
                DisplayName = "View Users",
                Description = "Allows viewing user details.",
                IsSystem = true,
            });
        }

        private async Task<IReadOnlyList<Role>> CreateSystemRolesAsync(IEnumerable<Permission> permissions, CancellationToken ct)
        {
            var roles = new List<Role>();
            var adminRole = new Role
            {
                IsSystem = true,
                Name = Constants.SystemRoles.Admin,
                DisplayName = Constants.AppName + " Administrator",
                Description = "System administrator role with full access to administration features.",
            };
            roles.Add(adminRole);
            var userRole = new Role
            {
                IsSystem = true,
                Name = Constants.SystemRoles.User,
                DisplayName = Constants.AppName + " User",
                Description = "Default user role with limited permissions.",
            };
            roles.Add(userRole);
            await dbContext.Roles.AddRangeAsync(roles, ct);
            await dbContext.SaveChangesAsync(ct);
            return roles;
        }

        private async Task LinkRolesToGroupsAsync(IEnumerable<Role> roles, IEnumerable<Group> groups, CancellationToken ct)
        {
            var adminRole = roles.First(r => r.Name == Constants.SystemRoles.Admin);
            var userRole = roles.First(r => r.Name == Constants.SystemRoles.User);
            var adminGroup = groups.First(g => g.Name == Constants.SystemGroups.Admin);
            var usersGroup = groups.First(g => g.Name == Constants.SystemGroups.Users);

            await dbContext.RoleGroups.AddRangeAsync(
            [
                new RoleGroup { RoleId = adminRole.Id, GroupId = adminGroup.Id },
                new RoleGroup { RoleId = userRole.Id, GroupId = usersGroup.Id }
            ], ct);
            await dbContext.SaveChangesAsync(ct);
        }

        private async Task GrantPermissionsToRolesAsync(IEnumerable<Permission> permissions, IEnumerable<Role> roles, CancellationToken ct)
        {
            var adminRole = roles.First(r => r.Name == Constants.SystemRoles.Admin);
            var userRole = roles.First(r => r.Name == Constants.SystemRoles.User);

            var allPermissions = permissions.ToList();
            foreach (var permission in allPermissions)
            {
                await dbContext.PermissionRoles.AddAsync(new PermissionRole
                {
                    RoleId = adminRole.Id,
                    PermissionId = permission.Id
                }, ct);
            }

            var viewUsersPermission = allPermissions.First(p => p.Name == Constants.SystemPermissions.Users.View);
            await dbContext.PermissionRoles.AddAsync(new PermissionRole
            {
                RoleId = userRole.Id,
                PermissionId = viewUsersPermission.Id
            }, ct);

            await dbContext.SaveChangesAsync(ct);
        }
    }
}
