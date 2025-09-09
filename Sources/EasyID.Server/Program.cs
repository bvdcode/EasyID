using EasyID.Server.Database;
using EasyExtensions.AspNetCore.Extensions;
using EasyExtensions.EntityFrameworkCore.Extensions;
using EasyExtensions.AspNetCore.Authorization.Extensions;
using EasyExtensions.EntityFrameworkCore.Npgsql.Extensions;

namespace EasyID.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string[] corsOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
                ?? throw new ArgumentNullException(null, "Allowed origins cannot be null.");
            builder.Services.AddJwt(builder.Configuration);
            builder.Services.AddDefaultCorsWithOrigins(corsOrigins);
            builder.Services.AddPostgresDbContext<AppDbContext>(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();

            var app = builder.Build();
            app.UseCors().UseDefaultFiles();
            app.MapStaticAssets();
            app.UseAuthorization();
            app.UseAuthentication();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");
            app.ApplyMigrations<AppDbContext>();
            app.MapHealthChecks("/api/{version}/health");
            app.Run();
        }
    }
}