using System.Security.Claims;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoAPI.JWT.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string RefreshToken { get; set; }
        public ICollection<ToDoAPI.Model.ToDo> ToDoList { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }

    }
}
