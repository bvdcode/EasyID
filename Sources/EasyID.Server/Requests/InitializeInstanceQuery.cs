using MediatR;
using EasyID.Server.Database;
using EasyExtensions.Services;
using EasyID.Server.Models.Dto;

namespace EasyID.Server.Requests
{
    public class InitializeInstanceQuery : IRequest
    {
        public LoginRequestDto FirstLoginRequest { get; set; } = null!;
    }

    public class IInitializeInstanceQueryHandler(IConfiguration _configuration, AppDbContext _dbContext) : IRequestHandler<InitializeInstanceQuery>
    {
        private readonly string _pepper = _configuration.GetValue<string>("Encryption:Pepper")
            ?? throw new ArgumentNullException("Encryption:Pepper", "Encryption key cannot be null.");
        private readonly int _passwordVersion = _configuration.GetValue<int>("Encryption:PasswordVersion", 1);

        public async Task Handle(InitializeInstanceQuery request, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(request.FirstLoginRequest.Username, nameof(request.FirstLoginRequest.Username));
            ArgumentException.ThrowIfNullOrWhiteSpace(request.FirstLoginRequest.Password, nameof(request.FirstLoginRequest.Password));
            Pbkdf2PasswordHashService pbk = new(_pepper);
            string hashedPassword = pbk.Hash(request.FirstLoginRequest.Password);
            string email = request.FirstLoginRequest.Username.Contains('@') ? request.FirstLoginRequest.Username : "admin@localhost";
            string username = request.FirstLoginRequest.Username.Contains('@') ? "admin" : request.FirstLoginRequest.Username;
            User user = new()
            {
                Email = email,
                FailedCount = 0,
                FirstName = "Admin",
                LastName = null,
                ForceReset = false,
                LockoutUntil = null,
                MiddleName = null,
                PasswordPhc = hashedPassword,
                PasswordVersion = _passwordVersion,
                PhoneNumber = null,
                Username = username
            };
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
