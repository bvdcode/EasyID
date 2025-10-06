using MediatR;
using EasyID.Server.Database;
using EasyID.Server.Models.Dto;
using EasyExtensions.Abstractions;
using EasyID.Server.Database.Models;

namespace EasyID.Server.Requests
{
    public class InitializeInstanceQuery : IRequest
    {
        public LoginRequestDto FirstLoginRequest { get; set; } = null!;
    }

    public class IInitializeInstanceQueryHandler(IPasswordHashService _hashService, AppDbContext _dbContext) : IRequestHandler<InitializeInstanceQuery>
    {
        public async Task Handle(InitializeInstanceQuery request, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(request.FirstLoginRequest.Username, nameof(request.FirstLoginRequest.Username));
            ArgumentException.ThrowIfNullOrWhiteSpace(request.FirstLoginRequest.Password, nameof(request.FirstLoginRequest.Password));
            string hashedPassword = _hashService.Hash(request.FirstLoginRequest.Password);
            string email = request.FirstLoginRequest.Username.Contains('@') ? request.FirstLoginRequest.Username : "admin@localhost";
            string username = request.FirstLoginRequest.Username.Contains('@') ? "admin" : request.FirstLoginRequest.Username;
            User user = new()
            {
                Email = email,
                FailedCount = 0,
                LastName = null,
                MiddleName = null,
                PhoneNumber = null,
                Username = username,
                FirstName = "Admin",
                PasswordPhc = hashedPassword,
                PasswordVersion = _hashService.PasswordHashVersion,
            };
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var groups = await CreateSystemGroupsAsync(user);
            var roles = await CreateSystemRolesAsync(groups);
            var permissions = await CreateSystemPermissionsAsync(roles);
        }

        private async Task<IList<Permission>> CreateSystemPermissionsAsync()
        {
            List<Permission> permissions = [];

            permissions.Add(new Permission
            {
                Name = "*",
                DisplayName = "All Permissions",
                Description = "Grants all permissions.",
                IsSystem = true,
            });

            permissions.Add(new Permission
            {
                Name = Constants.AppName.ToLower() + ".*",
                DisplayName = Constants.AppName + " All Permissions",
                Description = "Grants all permissions for " + Constants.AppName + ".",
                IsSystem = true,
            });

            AddUserPermissions(permissions);

            await _dbContext.Permissions.AddRangeAsync(permissions);
            await _dbContext.SaveChangesAsync();
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

        private async Task<IList<Role>> CreateSystemRolesAsync(IEnumerable<Permission> permissions)
        {
            List<Role> roles = [];
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
            await _dbContext.Roles.AddRangeAsync(roles);
            await _dbContext.SaveChangesAsync();

            foreach (var permission in permissions)
            {
                var adminRolePermission = new RolePermission
                {
                    RoleId = adminRole.Id,
                    PermissionId = permission.Id,
                };
                await _dbContext.RolePermissions.AddAsync(adminRolePermission);
            }
            var userViewUsersPermission = permissions.First(p => p.Name == Constants.SystemPermissions.Users.View);
            var userRolePermission = new RolePermission
            {
                RoleId = userRole.Id,
                PermissionId = userViewUsersPermission.Id,
            };
            await _dbContext.RolePermissions.AddAsync(userRolePermission);
            await _dbContext.SaveChangesAsync();
            return roles;
        }

        private async Task<IEnumerable<Group>> CreateSystemGroupsAsync()
        {
            var adminGroup = new Group
            {
                IsSystem = true,
                Name = Constants.SystemGroups.Admin,
                DisplayName = Constants.AppName + " Administrators",
                Description = "System administrators group with full access to administration features.",
            };
            var userGroup = new Group
            {
                IsSystem = true,
                Name = Constants.SystemGroups.Users,
                DisplayName = Constants.AppName + " Users",
                Description = "Default users group with limited permissions.",
            };
            await _dbContext.Groups.AddRangeAsync([adminGroup, userGroup]);
            await _dbContext.SaveChangesAsync();
            return [adminGroup, userGroup];
        }
    }
}
