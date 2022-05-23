using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

using ToDoAPI.Data;
using ToDoAPI.JWT.Model;

namespace ToDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ToDoAPIContext _context;
        private readonly IUserService _userService;
        public UsersController(ToDoAPIContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllAsync()
        {
            var resultDTO = new List<UserDTO>();
            var result = await _context.Users.ToListAsync();
            foreach (var item in result)
                resultDTO.Add(UserToDTO(item));
            return resultDTO;

        }


        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserDTO userCredentials)
        {

            if (userCredentials == null) throw new Exception.InvalidCredentialsException();
            if (_userService.IsCreated(userCredentials))
                return BadRequest("User is already created");

            var NewUser = new User()
            {
                Username = userCredentials.Username,
                Password = userCredentials.Password,

            };

            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == "Common");
            UserRole userRole = new()
            {
                Role = role,
                User = NewUser
            };

            _context.Users.Add(NewUser);
            _context.UserRoles.Add(userRole);


            await _context.SaveChangesAsync();
            return  CreatedAtAction("CreateUser", userCredentials.Username);
        }
        private UserDTO UserToDTO(User user) =>
        new UserDTO()
        {
            Username = user.Username,
            Password = user.Password
        };
    }
}
