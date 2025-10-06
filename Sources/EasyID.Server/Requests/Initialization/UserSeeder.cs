using EasyID.Server.Database;
using EasyExtensions.Abstractions;
using EasyID.Server.Database.Models;

namespace EasyID.Server.Requests.Initialization
{
    internal static class UserSeeder
    {
        public static async Task<User> SeedInitialAdminUserAsync(
            AppDbContext dbContext,
            IPasswordHashService hashService,
            string username,
            string password,
            CancellationToken cancellationToken)
        {
            bool usernameIsEmail = username.Contains('@');
            string email = usernameIsEmail ? username : "admin@localhost";
            string actualUsername = usernameIsEmail ? "admin" : username;

            var user = new User
            {
                Email = email,
                Username = actualUsername,
                FirstName = "Admin",
                FailedCount = 0,
                PasswordPhc = hashService.Hash(password),
                PasswordVersion = hashService.PasswordHashVersion,
            };

            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return user;
        }
    }
}
