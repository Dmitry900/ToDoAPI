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
        public async Task<IActionResult> Authenticate(AuthRequest request)
        {
            try
            {
                var response = await _authService.AuthenticateAsync(request);
                return Ok(response);
            }
            catch 
            {
                return Unauthorized();
            }
        }
    }
}
