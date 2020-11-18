using System;
using System.Collections.Generic;

namespace NebulaMigration.ViewModels
{
    /// <summary>
    /// Order view model.
    /// </summary>
    public class OrderViewModel
    {
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        public Guid Id;

        /// <summary>
        /// Коллекция блюд
        /// </summary>
        public IEnumerable<DishViewModel> Dishes;

        /// <summary>
        /// Номер стола
        /// </summary>
        public int Table;
        
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Комментарий к заказу
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Флаг экспорта заказа во внешнюю систему
        /// </summary>
        public bool IsExportRequested { get; set; }
    }
}