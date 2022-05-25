using ToDoAPI.JWT.Resources;
using ToDoAPI.JWT.Model;

namespace ToDoAPI.Services.Interfaces
{
    public interface IUserService
    {
        public bool IsCreated(UserDTO user);
        public Task<User> ValidateCredentialsAsync(AuthRequest authRequest);
        public User SetRefreshToken(User user, string token);
        public Task<User> GetUserAsync(string username);

    }
}
