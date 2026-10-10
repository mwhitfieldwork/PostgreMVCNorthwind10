using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NWCodeFirstMVC.Domain.Contracts;
using System.Security.Claims;
using System.Text;

namespace NWCodeFirstMVC.Infrastructure.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(string userId, string email, string? name = null)
        {
            var key = new SymmetricSecurityKey(Convert.FromBase64String(config["Jwt:Key"]!));

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Email, email)
        };
            if (!string.IsNullOrEmpty(name))
                claims.Add(new(JwtRegisteredClaimNames.Name, name));

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = config["Jwt:Issuer"],
                Audience = config["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(config.GetValue<int>("Jwt:ExpiresMinutes")),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            return new JsonWebTokenHandler().CreateToken(descriptor);
        }
    }
}
