using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoAPI.JWT.Resources
{
    public class AuthRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
