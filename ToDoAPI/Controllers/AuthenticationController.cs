using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoAPI.JWT.Services.Interfaces;
using ToDoAPI.JWT.Resources;
namespace ToDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Authenticate(AuthRequest request)
        {
            //try
            //{
                var response = _authService.Authenticate(request);
                return Ok(response);
            //}
            //catch (Exception)
            //{
            //    return Unauthorized();
            //}
        }
    }
}
