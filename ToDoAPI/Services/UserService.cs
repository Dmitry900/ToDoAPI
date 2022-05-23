using ToDoAPI.JWT.Resources;
using ToDoAPI.Services.Interfaces;
using ToDoAPI.JWT.Model;
using ToDoAPI.Exception;
using ToDoAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ToDoAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ToDoAPIContext _context;

        public UserService(ToDoAPIContext context)
        {
            _context = context;
        }
        public bool IsCreated(UserDTO userDTO)
        {
            User user =  _context.Users.SingleOrDefault(x => x.Username == userDTO.Username && x.Password == userDTO.Password);
            if (user != null) return true;
            else return false;
        }
        public async Task<User> ValidateCredentialsAsync(AuthRequest authRequest)
        {
            User user = await _context.Users
                .Where(x => x.Username == authRequest.Username && x.Password == authRequest.Password)
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync();    
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
