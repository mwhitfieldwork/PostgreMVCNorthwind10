using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain
{
    public class GoogleAuthModels
    {
        public record GoogleTokenExchangeRequest(string Code);

        public record GoogleTokenResponse(
            [property: JsonPropertyName("access_token")] string AccessToken,
            [property: JsonPropertyName("expires_in")] int ExpiresIn,
            [property: JsonPropertyName("id_token")] string IdToken,
            [property: JsonPropertyName("scope")] string Scope,
            [property: JsonPropertyName("token_type")] string TokenType,
            [property: JsonPropertyName("refresh_token")] string? RefreshToken
        );

        public class GoogleUserInfo
        {
            public string Sub { get; set; } = default!;
            public string Email { get; set; } = default!;
            public bool EmailVerified { get; set; }
            public string Name { get; set; } = default!;
            public string Picture { get; set; } = default!;
        }
    }
}
