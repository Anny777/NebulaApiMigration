using NebulaMigration.Models.Enums;
using System;

namespace NebulaMigration.ViewModels
{
    public class DishViewModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Название блюда
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Состав блюда
        /// </summary>
        public string Consist { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Комментарий к блюду
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Цена блюда
        /// </summary>
        public decimal Price { get; set; }
    }
}