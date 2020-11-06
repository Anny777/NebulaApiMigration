using System.ComponentModel.DataAnnotations;

namespace NebulaMigration.Options
{
    /// <summary>
    /// NebulaApiOptions.
    /// </summary>
    public class NebulaApiOptions
    {
        /// <summary>
        /// Gets or sets the security key.
        /// </summary>
        /// <value>
        /// The security key.
        /// </value>
        public string SecurityKey { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; set; }
    }
}
