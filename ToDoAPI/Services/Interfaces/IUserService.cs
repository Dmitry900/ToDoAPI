using ToDoAPI.JWT.Resources;
using ToDoAPI.JWT.Model;

namespace ToDoAPI.Services.Interfaces
{
    public interface IUserService
    {
        public User ValidateCredentials(AuthRequest authRequest);

    }
}
