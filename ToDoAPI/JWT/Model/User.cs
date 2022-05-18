using System.Security.Claims;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoAPI.JWT.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public ICollection<ToDoAPI.Model.ToDo> ToDoList { get; set; }
        //public ICollection<string> Roles { get; set; }
        //[NotMapped]
        //public IEnumerable<Claim> ClaimList 
        //{
        //    get
        //    {
        //        var claims = new List<Claim> { new Claim(ClaimTypes.Name, Username) };
        //        claims.AddRange(Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        //        return claims;
        //    }
        //}
    }
}
