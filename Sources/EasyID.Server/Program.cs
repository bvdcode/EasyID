using EasyID.Server.Database;
using Microsoft.EntityFrameworkCore;
using EasyExtensions.EntityFrameworkCore.Extensions;

namespace EasyID.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string[] corsOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
                ?? throw new ArgumentNullException(null, "Allowed origins cannot be null.");
            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlite(builder.Configuration["SqliteConnectionString"]));

            var app = builder.Build();
            app.UseCors().UseDefaultFiles();
            app.MapStaticAssets();
            app.UseAuthorization();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");
            app.ApplyMigrations<AppDbContext>();
            app.MapHealthChecks("/api/{version}/health");
            app.Run();
        }
    }
}