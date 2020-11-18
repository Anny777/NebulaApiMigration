using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NebulaMigration.Models;
using NebulaMigration.Models.Enums;
using NebulaMigration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NebulaMigration.Controllers
{
    /// <summary>
    /// Dish controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly ApplicationContext db;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DishController"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <exception cref="ArgumentNullException">db.</exception>
        public DishController(ApplicationContext db)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        //public void SetToken(string token)
        //{
        //    Formula360Connection.SetToken(token);
        //}

        /// <summary>
        /// Получение списка блюд
        /// /// </summary>
        /// <returns>List of dish view model.</returns>
        [HttpGet]
        [AllowAnonymous]
        [Authorize(Roles = "Waiter, Bartender, Cook, Admin")]
        public ActionResult<List<DishViewModel>> Get()
        {
            return this.db.Dishes.Select(this.mapper.Map<DishViewModel>).OrderBy(b => b.Name).ToList();
        }

        /// <summary>
        /// Смена состояния блюда на готовое
        /// </summary>
        /// <param name="id">The id of dish.</param>
        /// <param name="dishState">The state of dish.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>Order.</returns>
        [HttpPost("SetState")]
        [Authorize(Roles = "Admin, Bartender, Cook, Waiter")]
        public async Task<ActionResult<OrderViewModel>> SetState(int id, DishState dishState, CancellationToken ct)
        {
            try
            {
                var dish = this.db.CookingDishes.Find(id);
                if (dish == null)
                {
                    return this.NotFound("Блюдо не найдено!");
                }

                dish.DishState = dishState;
                await db.SaveChangesAsync(ct).ConfigureAwait(false);
                return Ok(this.mapper.Map<OrderViewModel>(dish));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Добавление блюда
        /// </summary>
        /// <param name="dish">объект блюда</param>
        /// <param name="idOrder">идентификатор заказа</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>Order view model.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin, Bartender, Waiter")]
        public async Task<ActionResult<OrderViewModel>> Post(DishViewModel dish, int idOrder, CancellationToken ct)
        {
            try
            {
                var custom = db.Customs.Find(idOrder);
                var currentDish = db.Dishes.Find(dish.Id);

                var newDish = new CookingDish
                {
                    IsActive = true,
                    Dish = currentDish,
                    DishState = DishState.InWork,
                    Comment = dish.Comment,
                };
                custom.CookingDishes.ToList().Add(newDish);
                await db.SaveChangesAsync(ct).ConfigureAwait(false);
                return Ok(this.mapper.Map<OrderViewModel>(custom));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Synchronizes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="token">The token.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>Action result.</returns>
        [HttpPost("Sync")]
        public async Task<ActionResult> Sync(SyncModel data, string token, CancellationToken ct)
        {
            if (!string.Equals("d3a71c3d-abd2-4833-9686-e5c8818c9054", token, StringComparison.InvariantCultureIgnoreCase))
                return BadRequest("Доступ запрещен");

            if (data == null)
                return BadRequest("Не переданы данные");

            if (data.Categories == null || data.Categories.Length == 0)
                return BadRequest("Передан пустой список категорий");

            if (data.Goods == null || data.Goods.Length == 0)
                return BadRequest("Передан пустой список блюд");

            this.db.Categories.AsParallel().ForAll(c => c.IsActive = false);
            foreach (var category in data.Categories)
            {
                var current = db.Categories.FirstOrDefault(c => c.ExternalId == category.ID);
                if (current == null)
                {
                    var newCategory = new Category
                    {
                        ExternalId = category.ID,
                        IsActive = true,
                        Name = category.Name,
                        Code = category.Code,
                        WorkshopType = WorkshopType.Kitchen,
                    };
                    db.Categories.Add(newCategory);
                    break;
                }

                current.ExternalId = category.ID;
                current.Name = category.Name;
                current.Code = category.Code;
                current.IsActive = true;
            }

            await db.SaveChangesAsync(ct).ConfigureAwait(false);

            db.Dishes.AsParallel().ForAll(c => c.IsActive = false);
            var categories = db.Categories.ToArray();
            foreach (var dish in data.Goods)
            {
                var price = dish.PriceOut2.HasValue ? decimal.Parse(dish.PriceOut2.Value.ToString()) : 0;
                var category = categories.Single(c => c.ExternalId == dish.GroupID || c.Code == dish.GroupID.ToString());

                var current = db.Dishes.FirstOrDefault(c => c.ExternalId == dish.ID);
                if (current == null)
                {
                    var newDish = new Dish
                    {
                        IsActive = true,
                        Consist = dish.Description,
                        Unit = dish.Measure1,
                        IsAvailable = true,
                        Name = dish.Name,
                        Price = price,
                        Category = category,
                        ExternalId = dish.ID,
                    };
                    db.Dishes.Add(newDish);
                    break;
                }

                current.ExternalId = dish.ID;
                current.Category = category;
                current.Name = dish.Name;
                current.Consist = dish.Description;
                current.IsAvailable = true;
                current.Price = price;
                current.Unit = dish.Measure1;
                current.IsActive = true;
            }

            await db.SaveChangesAsync(ct).ConfigureAwait(false);
            return Ok();
        }
    }
}
