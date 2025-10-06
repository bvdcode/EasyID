using EasyID.Server.Middleware;

namespace EasyID.Server.Extensions
{
    public static class UsersMeRewriteExtensions
    {
        public static IServiceCollection AddUsersMeRewrite(this IServiceCollection services, Action<UsersMeRewriteOptions>? configure = null)
        {
            if (configure != null)
            {
                services.Configure(configure);
            }
            else
            {
                services.Configure<UsersMeRewriteOptions>(_ => { });
            }
            return services;
        }

        public static IApplicationBuilder UseUsersMeRewrite(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UsersMeRewriteMiddleware>();
        }
    }
}
