namespace NebulaMigration.Models
{
    public class AuthenticateResponse
    {
        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            Username = user.UserName;
            Access_token = token;
            Token_type = "Bearer";
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string Access_token { get; set; }
        public string Token_type { get; set; }
    }
}
