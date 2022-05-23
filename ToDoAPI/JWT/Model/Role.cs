
using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.JWT.Model
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<UserRole> UsersRole { get; set; } 
    }
}
