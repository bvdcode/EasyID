using MediatR;
using EasyID.Server.Database;
using EasyID.Server.Models.Dto;
using EasyExtensions.Abstractions;
using Microsoft.EntityFrameworkCore;
using EasyID.Server.Requests.Initialization;

namespace EasyID.Server.Requests
{
    public class InitializeInstanceQuery : IRequest
    {
        public LoginRequestDto FirstLoginRequest { get; set; } = null!;
    }

    public class InitializeInstanceQueryHandler(IPasswordHashService hashService, AppDbContext dbContext)
        : IRequestHandler<InitializeInstanceQuery>
    {
        public async Task Handle(InitializeInstanceQuery request, CancellationToken cancellationToken)
        {
            if (await dbContext.Users.AnyAsync(cancellationToken))
            {
                return; // Already initialized.
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(request.FirstLoginRequest.Username, nameof(request.FirstLoginRequest.Username));
            ArgumentException.ThrowIfNullOrWhiteSpace(request.FirstLoginRequest.Password, nameof(request.FirstLoginRequest.Password));

            var user = await UserSeeder.SeedInitialAdminUserAsync(
                dbContext,
                hashService,
                request.FirstLoginRequest.Username,
                request.FirstLoginRequest.Password,
                cancellationToken);

            var groups = await GroupSeeder.SeedSystemGroupsAsync(dbContext, user, cancellationToken);
            var permissions = await PermissionSeeder.SeedSystemPermissionsAsync(dbContext, cancellationToken);
            var roles = await RoleSeeder.SeedSystemRolesAsync(dbContext, cancellationToken);

            await RoleSeeder.LinkRolesToGroupsAsync(dbContext, roles, groups, cancellationToken);
            await RoleSeeder.GrantPermissionsToRolesAsync(dbContext, permissions, roles, cancellationToken);
        }
    }
}
