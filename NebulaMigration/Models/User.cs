using Microsoft.AspNetCore.Identity;

namespace NebulaMigration.Models
{
    public class User : IdentityUser
    {
        public int OperatorId { get; set; }

        public string Access_token { get; set; }

        public string Token_type { get; set; }

        public int Expires_in { get; set; }

        public string Issued { get; set; }

        public string Expires { get; set; }
    }
}
