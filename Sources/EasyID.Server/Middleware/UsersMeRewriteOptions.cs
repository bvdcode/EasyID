using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace EasyID.Server.Middleware
{
    public class UsersMeRewriteOptions
    {
        public string[] ClaimTypesOrder { get; set; } =
        [
            JwtRegisteredClaimNames.Sub,
            ClaimTypes.NameIdentifier,
            ClaimTypes.Sid
        ];
    }
}
