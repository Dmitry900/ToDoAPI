using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.JWT.Model;

namespace ToDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ToDoAPIContext _context;
        public UsersController(ToDoAPIContext context)
        {
            _context = context;
        }
        [HttpGet]
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
            var NewUser = new User()
            {
                Username = userCredentials.Username,
                Password = userCredentials.Password,
            };
            _context.Users.Add(NewUser);
            await _context.SaveChangesAsync();
            return  CreatedAtAction("CreateUser", userCredentials);
        }
        private UserDTO UserToDTO(User user) =>
        new UserDTO()
        {
            Username = user.Username,
            Password = user.Password
        };
    }
}
