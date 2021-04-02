namespace NebulaMigration.ViewModels
{
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets of new password.
        /// </summary>
        public string NewPassword { get; set; }
    }
}
