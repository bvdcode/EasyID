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

    public class InitializeInstanceQueryHandler : IRequestHandler<InitializeInstanceQuery>
    {
        private readonly IPasswordHashService _hashService;
        private readonly AppDbContext _dbContext;

        public InitializeInstanceQueryHandler(IPasswordHashService hashService, AppDbContext dbContext)
        {
            _hashService = hashService;
            _dbContext = dbContext;
        }

        public async Task Handle(InitializeInstanceQuery request, CancellationToken cancellationToken)
        {
            if (await _dbContext.Users.AnyAsync(cancellationToken))
            {
                return; // Already initialized.
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(request.FirstLoginRequest.Username, nameof(request.FirstLoginRequest.Username));
            ArgumentException.ThrowIfNullOrWhiteSpace(request.FirstLoginRequest.Password, nameof(request.FirstLoginRequest.Password));

            var user = await UserSeeder.SeedInitialAdminUserAsync(
                _dbContext,
                _hashService,
                request.FirstLoginRequest.Username,
                request.FirstLoginRequest.Password,
                cancellationToken);

            var groups = await GroupSeeder.SeedSystemGroupsAsync(_dbContext, user, cancellationToken);
            var permissions = await PermissionSeeder.SeedSystemPermissionsAsync(_dbContext, cancellationToken);
            var roles = await RoleSeeder.SeedSystemRolesAsync(_dbContext, cancellationToken);

            await RoleSeeder.LinkRolesToGroupsAsync(_dbContext, roles, groups, cancellationToken);
            await RoleSeeder.GrantPermissionsToRolesAsync(_dbContext, permissions, roles, cancellationToken);
        }
    }
}
