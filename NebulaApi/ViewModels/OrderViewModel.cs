using NebulaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NebulaApi.ViewModels
{
    public class OrderViewModel
    {
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        public int Id;
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