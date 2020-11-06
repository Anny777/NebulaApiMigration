namespace NebulaMigration.Models
{
    using System;
    using NebulaMigration.Models.Enums;
    using NebulaMigration.ViewModels;

    public class CookingDish
    {
        bool isActive;
        DateTime createdDate;
        Dish dish;
        DishState dishState;
        Custom custom;
        string comment;

        public CookingDish()
        {

        }

        public CookingDish(
            bool isActive,
            Dish dish,
            DishState dishState,
            string comment)
        {
            this.isActive = isActive;
            this.createdDate = DateTime.Now;
            this.dish = dish;
            this.dishState = dishState;
            this.comment = comment;
        }

        public Guid Id { get; set; }
        public bool IsActive => this.isActive;
        public DateTime CreatedDate => this.createdDate;
        public Dish Dish => this.dish;
        public DishState DishState => this.dishState;
        public Custom Custom => this.custom;
        public string Comment => this.comment;

        public void SetState(DishState dishState)
        {
            this.dishState = dishState;
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