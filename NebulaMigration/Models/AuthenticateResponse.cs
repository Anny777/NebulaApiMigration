namespace NebulaMigration.Models
{
    using System.Text.Json.Serialization;

    public class AuthenticateResponse
    {
        public AuthenticateResponse(User user, string token)
        {
            this.Id = user.Id;
            this.Username = user.UserName;
            this.Access_token = token;
            this.Token_type = "Bearer";
        }

        public AuthenticateResponse()
        {
        }
        
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }
        
        [JsonPropertyName("access_token")]
        public string Access_token { get; set; }

        [JsonPropertyName("token_type")]
        public string Token_type { get; set; }
    }
}
