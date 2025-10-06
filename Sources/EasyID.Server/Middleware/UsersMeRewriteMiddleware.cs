using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace EasyID.Server.Middleware
{
    public sealed class UsersMeRewriteOptions
    {
        public string[] ClaimTypesOrder { get; set; } =
        [
            "sub",
            "uid",
            ClaimTypes.NameIdentifier,
            ClaimTypes.Sid
        ];
    }

    public sealed class UsersMeRewriteMiddleware(
        ILogger<UsersMeRewriteMiddleware> logger,
        IOptions<UsersMeRewriteOptions> options,
        RequestDelegate next)
    {
        private const string MeSegment = "me";
        private readonly UsersMeRewriteOptions _options = options.Value;
        private readonly StringComparison _cmp = StringComparison.OrdinalIgnoreCase;

        public async Task InvokeAsync(HttpContext context)
        {
            var originalPath = context.Request.Path.Value ?? string.Empty;
            if (string.IsNullOrEmpty(originalPath) || !originalPath.Contains(MeSegment, _cmp))
            {
                await next(context);
                return;
            }

            // Quick scan for standalone segment; avoid allocation if not present.
            if (!PathContainsStandaloneMe(originalPath))
            {
                await next(context);
                return;
            }

            if (!(context.User?.Identity?.IsAuthenticated ?? false))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            if (!TryResolveUserId(context.User, out var userId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var rewritten = RewritePath(originalPath, userId);
            if (!rewritten.Equals(originalPath, _cmp))
            {
                context.Items["OriginalPath"] = originalPath;
                context.Request.Path = new PathString(rewritten);
                logger.LogDebug("UsersMeRewrite: {Original} -> {Rewritten}", originalPath, rewritten);
            }

            await next(context);
        }

        private bool TryResolveUserId(ClaimsPrincipal user, out Guid userId)
        {
            userId = Guid.Empty;
            string? idStr = null;
            foreach (var claimType in _options.ClaimTypesOrder)
            {
                idStr = user.FindFirst(claimType)?.Value;
                if (!string.IsNullOrEmpty(idStr))
                {
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(idStr) || !Guid.TryParse(idStr, out userId))
            {
                logger.LogWarning(
                    "UsersMeRewrite: cannot resolve GUID from claims. Claims tried: {Claims}",
                    string.Join(",", _options.ClaimTypesOrder));
                return false;
            }

            return true;
        }

        private static bool PathContainsStandaloneMe(string path)
        {
            // Consider leading, trailing, or surrounded by '/'
            // Fast char iteration without allocations.
            var span = path.AsSpan();
            for (int i = 0; i < span.Length; i++)
            {
                if (char.ToLowerInvariant(span[i]) != 'm')
                {
                    continue;
                }
                if (i + 1 >= span.Length || char.ToLowerInvariant(span[i + 1]) != 'e')
                {
                    continue;
                }

                int start = i;
                int endExclusive = i + 2;

                bool boundaryBefore = start == 0 || span[start - 1] == '/';
                bool boundaryAfter = endExclusive == span.Length || span[endExclusive] == '/';

                if (boundaryBefore && boundaryAfter)
                {
                    return true;
                }
            }
            return false;
        }

        private string RewritePath(string originalPath, Guid userId)
        {
            var userIdStr = userId.ToString("D");
            var hadTrailingSlash = originalPath.EndsWith('/');

            // Split into segments ignoring empty (leading slash) then recompose.
            var segments = originalPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var modified = false;
            for (int i = 0; i < segments.Length; i++)
            {
                if (segments[i].Equals(MeSegment, _cmp))
                {
                    segments[i] = userIdStr;
                    modified = true;
                }
            }

            if (!modified)
            {
                return originalPath; // No change.
            }

            var rebuilt = "/" + string.Join('/', segments);
            if (hadTrailingSlash && !rebuilt.EndsWith('/'))
            {
                rebuilt += "/";
            }
            return rebuilt;
        }
    }
}
