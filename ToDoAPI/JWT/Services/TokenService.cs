using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ToDoAPI.JWT.Services.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Cryptography;
using ToDoAPI.JWT.Model;
using ToDoAPI.JWT.Resources;

namespace ToDoAPI.JWT.Services
{
    public class TokenService : ITokenService
    {
        private readonly EncryptionKey _encryptionKey;

        public TokenService(IOptions<EncryptionKey> options)
        {
            _encryptionKey = options.Value;
        }

        public async Task<string> GetAccessTokenAsync(User user)
        {
            var  tokenDescriptor = await GetTokenDescriptor(user);
            var tokenHandler =  new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);

            return token;
        }
        public IEnumerable<Claim> DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            return jwt.Claims;
        }
        private async Task<SecurityTokenDescriptor> GetTokenDescriptor(User user)
        {
            const int expiringMinutes = 180;
            byte[] securityKey = await Task.Factory.StartNew(() => Encoding.UTF8.GetBytes(_encryptionKey.Key));

            var claims = GetClaims(user);
            var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(expiringMinutes),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenDescriptor;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_encryptionKey.Key)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public async Task<string> GetRefreshTokenAsync()
        {
            var randomNumber = new byte[64];
            var rng = await Task.Factory.StartNew(() => RandomNumberGenerator.Create());
            rng.GetBytes(randomNumber);
            var token = Convert.ToBase64String(randomNumber);
            return token;
        }

        private IEnumerable<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username)
            };
            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
            }

            return claims;
        }
    }
}
