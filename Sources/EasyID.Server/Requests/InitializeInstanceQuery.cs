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
        }
    }
}
