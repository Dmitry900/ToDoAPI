using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ToDoAPI.JWT.Services.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
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

        public string GetToken(User user)
        {
            SecurityTokenDescriptor tokenDescriptor = GetTokenDescriptor(user);
            var tokenHandler = new JwtSecurityTokenHandler();
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
        private SecurityTokenDescriptor GetTokenDescriptor(User user)
        {
            const int expiringDays = 7;

            byte[] securityKey = Encoding.UTF8.GetBytes(_encryptionKey.Key);
            var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);
            var claims = GetClaims(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(expiringDays),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenDescriptor;
        }
        private IEnumerable<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username)
            };

            return claims;
        }


    }
}
