namespace NebulaMigration.Commands
{
    using Models.Enums;

    /// <summary>
    /// Команда на создание категории.
    /// </summary>
    public class CreateCategoryCommand
    {
        
        /// <summary>
        /// ExternalId.
        /// </summary>
        public int ExternalId { get; set; }
        
        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Code.
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Workshop type.
        /// </summary>
        public WorkshopType WorkshopType { get; set; }
    }
}