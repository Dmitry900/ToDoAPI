using ToDoAPI.JWT.Model;
using System.Security.Claims;
using ToDoAPI.JWT.Resources;
namespace ToDoAPI.JWT.Services.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GetTokenAsync(User user);
        public IEnumerable<Claim> DecodeToken(string token);
    }
}
