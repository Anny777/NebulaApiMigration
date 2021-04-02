namespace NebulaMigration.Models
{
    /// <summary>
    /// Order for export.
    /// </summary>
    public class ExportOrder
    {
        /// <summary>
        /// Gets or sets the table number.
        /// </summary>
        /// <value>
        /// The table number.
        /// </value>
        public string TableNumber { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public int OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the dishes.
        /// </summary>
        /// <value>
        /// The dishes.
        /// </value>
        public ExportDish[] Dishes { get; set; }
    }
}
