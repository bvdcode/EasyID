using Microsoft.AspNetCore.Mvc;

namespace EasyID.Server.Controllers
{
    [ApiController]
    public class OpenIdController : ControllerBase
    {
        [HttpGet("/.well-known/openid-configuration")]
        public IActionResult GetOpenIdConfiguration()
        {
            var issuer = $"{Request.Scheme}://{Request.Host.Value}";
            var configuration = new
            {
                issuer,
                authorization_endpoint = $"{issuer}/connect/authorize",
                jwks_uri = $"{issuer}/.well-known/jwks.json",
                response_types_supported = new[] { "code" },
                subject_types_supported = new[] { "public" },
                id_token_signing_alg_values_supported = new[] { "RS256" },
                token_endpoint = $"{issuer}/connect/token",
                userinfo_endpoint = $"{issuer}/connect/userinfo",
                claims_supported = new[] { "sub", "name", "preferred_username", "email", "email_verified" }
            };
            return Ok(configuration);
        }
    }
}
