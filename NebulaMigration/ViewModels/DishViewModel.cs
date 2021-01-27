using NebulaMigration.Models.Enums;
using System;

namespace NebulaMigration.ViewModels
{
    public class DishViewModel
    {
        public DishViewModel()
        {
            IsActive = true;
            CreatedDate = DateTime.Now;
        }
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Идентификатор заказанного блюда
        /// </summary>
        public Guid CookingDishId;
        
        /// <summary>
        /// Название блюда
        /// </summary>
        public string Name;
        
        /// <summary>
        /// Состав блюда
        /// </summary>
        public string Consist;
        
        /// <summary>
        /// Единица измерения
        /// </summary>
        public string Unit;
        
        /// <summary>
        /// Состояние блюда
        /// </summary>
        public DishState State;
        
        /// <summary>
        /// Комментарий к блюду
        /// </summary>
        public string Comment;
        
        /// <summary>
        /// Цех
        /// </summary>
        public WorkshopType WorkshopType;
        
        /// <summary>
        /// Цена блюда
        /// </summary>
        public decimal Price;
    }
}