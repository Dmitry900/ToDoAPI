using System.Security.Claims;
using ToDoAPI.Services;
using ToDoAPI.JWT.Resources;
using ToDoAPI.JWT.Services.Interfaces;
using ToDoAPI.Services.Interfaces;


namespace ToDoAPI.JWT.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public LoginService(IUserService userService, ITokenService tokenService, ITokenService refreshtokenservice)
        {
            this._userService = userService;
            this._tokenService = tokenService;

        }

        public async Task<AuthResponse> AuthenticateAsync(AuthRequest userCredentials)
        {
            var user = await _userService.ValidateCredentialsAsync(userCredentials);

            var securityToken =  await _tokenService.GetAccessTokenAsync(user);
            var refreshtoken = await _tokenService.GetRefreshTokenAsync();
            user = _userService.SetRefreshToken(user, refreshtoken);

            return new AuthResponse(user, securityToken, refreshtoken);
        }
        public async Task<AuthResponse> GetNewTokens(List<Claim> claims)
        {
            var user = await _userService.GetUserAsync(claims[0].Value);
            var userCredentials = new AuthRequest()
            {
                Password = user.Password,
                Username = user.Username
            };
            return await AuthenticateAsync(userCredentials);

        }

        public async void Revoke(string username)
        {
            var user = await _userService.GetUserAsync(username);
            _userService.SetRefreshToken(user, null);

                                        
        }
    }
}
