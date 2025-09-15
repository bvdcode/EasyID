using MediatR;
using EasyID.Server.Database;
using EasyExtensions.Services;
using EasyID.Server.Models.Dto;
using EasyID.Server.Database.Models;

namespace EasyID.Server.Requests
{
    public class InitializeInstanceQuery : IRequest
    {
        public LoginRequestDto FirstLoginRequest { get; set; } = null!;
    }

    public class IInitializeInstanceQueryHandler(Pbkdf2PasswordHashService _hashService, AppDbContext _dbContext) : IRequestHandler<InitializeInstanceQuery>
    {
        public async Task Handle(InitializeInstanceQuery request, CancellationToken cancellationToken)
        {
            var groups = await CreateSystemGroupsAsync();

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

            foreach (var group in groups)
            {
                var userGroup = new GroupUser
                {
                    GroupId = group.Id,
                    UserId = user.Id,
                };
                await _dbContext.GroupUsers.AddAsync(userGroup, cancellationToken);
            }
        }

        private async Task<IEnumerable<Group>> CreateSystemGroupsAsync()
        {
            var adminGroup = new Group
            {
                IsSystem = true,
                Name = Constants.AdminGroupName,
                DisplayName = Constants.AppName + " Administrators",
                Description = "System administrators group with full access to administration features.",
            };
            var userGroup = new Group
            {
                IsSystem = true,
                Name = Constants.UsersGroupName,
                DisplayName = Constants.AppName + " Users",
                Description = "Default users group with limited permissions.",
            };
            await _dbContext.Groups.AddRangeAsync([adminGroup, userGroup]);
            await _dbContext.SaveChangesAsync();
            return [adminGroup, userGroup];
        }
    }
}
