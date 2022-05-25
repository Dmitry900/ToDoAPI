using ToDoAPI.JWT.Model;
using System.Security.Claims;
using ToDoAPI.JWT.Resources;
namespace ToDoAPI.JWT.Services.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GetAccessTokenAsync(User user);
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string? token);
        public Task<string> GetRefreshTokenAsync();
        public IEnumerable<Claim> DecodeToken(string token);
    }
}
