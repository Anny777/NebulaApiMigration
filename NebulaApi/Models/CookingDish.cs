using NebulaApi.ViewModels;
using ProjectOrderFood.Enums;

namespace NebulaApi.Models
{
    public class CookingDish : ModelBase
    {
        public virtual Dish Dish { get; set; }
        public virtual DishState DishState { get; set; }
        public virtual Custom Custom { get; set; }
        public string Comment { get; set; }

        public DishViewModel ToViewModel()
        {
            return new DishViewModel()
            {
                Id = Dish.Id,
                Name = Dish.Name,
                Consist = Dish.Consist,
                Price = Dish.Price,
                Unit = Dish.Unit,
                CookingDishId = Id,
                IsActive = IsActive,
                CreatedDate = CreatedDate,
                State = DishState,
                WorkshopType = Dish.Category.WorkshopType
            };
        }
    }


}