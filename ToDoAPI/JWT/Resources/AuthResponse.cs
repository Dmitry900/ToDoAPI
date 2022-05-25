using ToDoAPI.JWT.Model;
namespace ToDoAPI.JWT.Resources
{
    public class AuthResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public AuthResponse(User user, string accesstoken, string refreshtoken)
        {
            Id = user.Id;
            Username = user.Username;
            AccessToken = accesstoken;
            RefreshToken = refreshtoken;
        }
    }
}
