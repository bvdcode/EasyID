using EasyExtensions.Services;

namespace EasyID.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPbkdf2PasswordHashService(this IServiceCollection services, IConfiguration configuration)
        {
            const string pepperKey = "Encryption:Pepper";
            string pepper = configuration.GetValue<string>(pepperKey)
                ?? throw new ArgumentNullException(nameof(configuration), "Encryption key cannot be null.");
            const string passwordVersionKey = "Encryption:PasswordVersion";
            int passwordVersion = configuration.GetValue(passwordVersionKey, 1);
            ArgumentException.ThrowIfNullOrWhiteSpace(pepper, pepperKey);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(passwordVersion, passwordVersionKey);
            return services.AddSingleton(new Pbkdf2PasswordHashService(pepper, passwordVersion));
        }
    }
}
