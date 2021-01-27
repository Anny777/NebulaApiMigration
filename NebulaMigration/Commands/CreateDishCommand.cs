namespace NebulaMigration.Commands
{
    using System;

    /// <summary>
    /// Команда на создание блюда.
    /// </summary>
    public class CreateDishCommand
    {
        /// <summary>
        /// Внешний идентификатор.
        /// </summary>
        public int ExternalId { get; set; }
        
        /// <summary>
        /// Идентификатор категории.
        /// </summary>
        public Guid CategoryId { get; set; }
        
        /// <summary>
        /// Состав.
        /// </summary>
        public string Consist { get; set; }
        
        /// <summary>
        /// Единица измерения.
        /// </summary>
        public string Unit { get; set; }
        
        /// <summary>
        /// Есть ли в наличии.
        /// </summary>
        public bool IsAvailable { get; set; }
        
        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Цена.
        /// </summary>
        public decimal Price { get; set; }
    }
}