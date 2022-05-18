using ToDoAPI.JWT.Resources;
using ToDoAPI.Services.Interfaces;
using ToDoAPI.JWT.Model;
using ToDoAPI.Exception;
using ToDoAPI.Data;

namespace ToDoAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ToDoAPIContext _context;

        public UserService(ToDoAPIContext context)
        {
            _context = context;
        }

        public User ValidateCredentials(AuthRequest authRequest)
        {
            User user = _context.Users.SingleOrDefault(x => x.Username == authRequest.Username && x.Password == authRequest.Password);
            bool isValid = user != null && AreValidCredentials(authRequest, user);

            if (!isValid)
            {
                throw new InvalidCredentialsException();
            }
            else return user;
        }

        private bool AreValidCredentials(AuthRequest userCredentials, User user) 
            => user.Username == userCredentials.Username &&
                  user.Password == userCredentials.Password;

    }
}
