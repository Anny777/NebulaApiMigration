namespace NebulaMigration.Models
{
    /// <summary>
    /// Dish for export.
    /// </summary>
    public class ExportDish
    {
        /// <summary>
        /// Gets or sets the good identifier.
        /// </summary>
        /// <value>
        /// The good identifier.
        /// </value>
        public int GoodId { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public int Quantity { get; set; }
    }
}
