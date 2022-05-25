using System.Security.Claims;
using ToDoAPI.Services;
using ToDoAPI.JWT.Resources;

namespace ToDoAPI.JWT.Services.Interfaces
{
    public interface ILoginService
    {
        public Task<AuthResponse> AuthenticateAsync(AuthRequest userCredentials);
        public Task<AuthResponse> GetNewTokens(List<Claim> claims);
        public void Revoke(string username);

    }
}
