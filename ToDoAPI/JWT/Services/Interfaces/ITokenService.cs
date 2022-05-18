using ToDoAPI.JWT.Model;
using System.Security.Claims;
namespace ToDoAPI.JWT.Services.Interfaces
{
    public interface ITokenService
    {
        public string GetToken(User user);
        public IEnumerable<Claim> DecodeToken(string token);
    }
}
