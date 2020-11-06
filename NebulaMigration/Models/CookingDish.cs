namespace NebulaMigration.Models
{
    using System;
    using NebulaMigration.Models.Enums;
    using NebulaMigration.ViewModels;

    public class CookingDish
    {
        public CookingDish()
        {

        }

        public CookingDish(
            bool isActive,
            Dish dish,
            DishState dishState,
            string comment)
        {
            this.IsActive = isActive;
            this.CreatedDate = DateTime.Now;
            this.Dish = dish;
            this.DishState = dishState;
            this.Comment = comment;
        }

        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Dish Dish { get; set; }
        public DishState DishState { get; set; }
        public Custom Custom { get; set; }
        public string Comment { get; set; }

        public void SetState(DishState dishState)
        {
            this.DishState = dishState;
        }

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