namespace NebulaMigration.Models
{
    using System;
    using NebulaMigration.Models.Enums;

    public class CookingDish
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Dish Dish { get; set; }
        public DishState DishState { get; set; }
        public Custom Custom { get; set; }
        public string Comment { get; set; }
    }
}