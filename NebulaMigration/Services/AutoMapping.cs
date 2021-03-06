using AutoMapper;
using NebulaMigration.Models;
using NebulaMigration.ViewModels;

namespace NebulaMigration.Services
{
    using System;
    using System.Linq;
    using Commands;
    using Models.Enums;

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
            CreateMap<CreateCategoryCommand, Category>()
                .ForMember(c => c.CreatedDate, e => e.MapFrom(c => DateTime.Now))
                .ForMember(c => c.IsActive, e => e.MapFrom(c => true));

            CreateMap<CreateDishCommand, Dish>()
                .ForMember(c => c.CreatedDate, e => e.MapFrom(c => DateTime.Now))
                .ForMember(c => c.IsActive, e => e.MapFrom(c => true));

            CreateMap<CookingDish, CookingDishViewModel>()
                .ForMember(c => c.DishName, e => e.MapFrom(c => c.Dish.Name));

            CreateMap<Category, CategoryViewModel>();
            CreateMap<Custom, OrderViewModel>()
                .ForMember(
                    c => c.ReadyDishesCount,
                    m => m.MapFrom(c => c.CookingDishes.Count(cd => cd.DishState == DishState.Ready)));
            CreateMap<Dish, DishViewModel>();
            CreateMap<DishViewModel, Dish>();
            CreateMap<Dish, CookingDish>()
                .ForMember(c => c.Dish, a => a.MapFrom((q, w) => q));
        }
    }
}