using FluentValidation;
using EasyID.Server.Mappings;
using EasyID.Server.Database;
using EasyID.Server.Extensions;
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
            builder.Services.AddJwt(builder.Configuration)
                .AddExceptionHandler()
                .AddPbkdf2PasswordHashService(builder.Configuration)
                .AddAutoMapper(x => x.AddProfile<AppMappingProfile>())
                .AddValidatorsFromAssemblyContaining<Program>()
                .AddDefaultCorsWithOrigins(corsOrigins)
                .AddPostgresDbContext<AppDbContext>(builder.Configuration)
                .AddControllers().Services
                .AddHealthChecks().Services
                .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<Program>());

            var app = builder.Build();
            app.UseExceptionHandler();
            app.UseCors().UseDefaultFiles();
            app.MapStaticAssets();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");
            //app.ApplyMigrations<AppDbContext>();
            app.MapHealthChecks("/api/{version}/health");

            //var scope = app.Services.CreateScope();
            //var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //bool deleted = dbContext.Database.EnsureDeleted();
            //Console.WriteLine(deleted ? "Database deleted" : "!!! Database not deleted !!!");
            //dbContext.Database.EnsureCreated();
            //Console.WriteLine("Database created");

            app.Run();
        }
    }
}