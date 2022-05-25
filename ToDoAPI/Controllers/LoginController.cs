using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToDoAPI.JWT.Services.Interfaces;
using ToDoAPI.JWT.Resources;
using ToDoAPI.JWT.Model;

namespace ToDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _authService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public LoginController(ILoginService authService, ITokenService tokenService, IConfiguration configuration)
        {
            _authService = authService;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Authenticate")]
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
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenDTO token)
        {
            try
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(token.AccessToken);
                var response = await _authService.GetNewTokens(principal.Claims.ToList());
                return Ok(response);
            }
            catch
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            _authService.Revoke(username);
            return NoContent();
        }


    }
}
