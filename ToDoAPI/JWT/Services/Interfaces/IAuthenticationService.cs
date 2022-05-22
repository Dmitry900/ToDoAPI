using ToDoAPI.Services;
using ToDoAPI.JWT.Resources;

namespace ToDoAPI.JWT.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<AuthResponse> AuthenticateAsync(AuthRequest userCredentials);
    }
}
