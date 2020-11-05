using System.ComponentModel.DataAnnotations;

namespace NebulaApi.Models
{
    public class Dish : ModelBaseSync
    {
        public virtual Category Category { get; set; }
        public string Consist { get; set; }
        public string Unit { get; set; }
        public bool IsAvailable { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

    }
}