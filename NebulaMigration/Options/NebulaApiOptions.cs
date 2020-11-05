using System.ComponentModel.DataAnnotations;

namespace NebulaMigration.Options
{
    public class NebulaApiOptions
    {
        /// <summary>
        /// Gets or sets the security key.
        /// </summary>
        /// <value>
        /// The security key.
        /// </value>
        [Required]
        public string SecurityKey { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        [Required]
        public string ConnectionString { get; set; }
    }
}
