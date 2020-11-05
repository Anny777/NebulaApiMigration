using Microsoft.AspNet.Identity.EntityFramework;
using NebulaApi.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace NebulaApi.Models
{
    public class Custom : ModelBase
    {
        public Custom()
        {
            CookingDishes = new List<CookingDish>();
        }

        public virtual ICollection<CookingDish> CookingDishes { get; set; }

        public bool IsOpened { get; set; }
        public virtual ApplicationUser User { get; set; }
        public bool IsExportRequested { get; set; }
        public int TableNumber { get; set; }
        public string Comment { get; set; }

        public OrderViewModel ToViewModel()
        {
            return new OrderViewModel()
            {
                Id = Id,
                Table = TableNumber,
                Dishes = CookingDishes.Select(c => c.ToViewModel()),
                CreatedDate = CreatedDate,
                Comment = Comment,
                IsExportRequested = IsExportRequested
            };
        }
    }
}