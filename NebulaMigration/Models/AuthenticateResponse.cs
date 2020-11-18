namespace NebulaMigration.Models
{
    public class AuthenticateResponse
    {
        public AuthenticateResponse(User user, string token)
        {
            this.Id = user.Id;
            this.Username = user.UserName;
            this.Access_token = token;
            this.Token_type = "Bearer";
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string Access_token { get; set; }
        public string Token_type { get; set; }
    }
}
