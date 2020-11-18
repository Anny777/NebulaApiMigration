namespace NebulaMigration.Models
{
    /// <summary>
    /// Model for sync.
    /// </summary>
    public class SyncModel
    {
        /// <summary>
        /// Gets or sets the goods.
        /// </summary>
        /// <value>
        /// The goods.
        /// </value>
        public Good[] Goods { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public GoodsGroup[] Categories { get; set; }
    }
}
