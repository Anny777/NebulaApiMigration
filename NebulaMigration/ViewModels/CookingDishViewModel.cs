namespace NebulaMigration.ViewModels
{
    using System;
    using Models.Enums;

    /// <summary>
    /// Cooking dish view model
    /// </summary>
    public class CookingDishViewModel
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Name of dish.
        /// </summary>
        public string DishName { get; set; }
        
        /// <summary>
        /// Cooking state.
        /// </summary>
        public DishState DishState { get; set; }
    }
}