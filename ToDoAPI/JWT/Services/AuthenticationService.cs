using ToDoAPI.Services;
using ToDoAPI.JWT.Resources;
using ToDoAPI.JWT.Services.Interfaces;
using ToDoAPI.Services.Interfaces;


namespace ToDoAPI.JWT.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IUserService userService, ITokenService tokenService)
        {
            this._userService = userService;
            this._tokenService = tokenService;
        }

        public async Task<AuthResponse> AuthenticateAsync(AuthRequest userCredentials)
        {
            var user = await _userService.ValidateCredentialsAsync(userCredentials);

            var securityToken =  await _tokenService.GetTokenAsync(user);

            return new AuthResponse(user,securityToken);
        }
    }
}
