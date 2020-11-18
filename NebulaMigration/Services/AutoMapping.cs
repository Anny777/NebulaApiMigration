using AutoMapper;
using NebulaMigration.Models;
using NebulaMigration.ViewModels;

namespace NebulaMigration.Services
{
    /// <summary>
    /// Auto mapping.
    /// </summary>
    public class AutoMapping : Profile
    {
        /// <summary>
        /// ctor.
        /// </summary>
        public AutoMapping()
        {
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Custom, OrderViewModel>();
            CreateMap<Dish, DishViewModel>();
            CreateMap<DishViewModel, CookingDish>();
        }
    }
}