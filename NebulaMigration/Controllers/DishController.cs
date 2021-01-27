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
    using Commands;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

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
        /// <param name="mapper">The mapper.</param>
        /// <exception cref="ArgumentNullException">db.</exception>
        public DishController(ApplicationContext db, IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Получение списка блюд
        /// /// </summary>
        /// <returns>List of dish view model.</returns>
        [HttpGet]
#if !DEBUG
        [Authorize(Roles = "Waiter, Bartender, Cook, Admin")]
#endif
        public ActionResult<IEnumerable<DishViewModel>> Get()
        {
            return this.Ok(this.db
                .Dishes
                .OrderBy(b => b.Name)
                .ToArray()
                .Select(this.mapper.Map<DishViewModel>));
        }

        /// <summary>
        /// Добавляет новое блюдо в систему.
        /// </summary>
        /// <returns>Ответ.</returns>
        [HttpPost]
#if !DEBUG
        [Authorize(Roles = "Waiter, Bartender, Cook, Admin")]
#endif
        public async Task<ActionResult> Post(CreateDishCommand dish, CancellationToken cancellationToken)
        {
            var currentDish = await this.db
                .Dishes
                .FirstOrDefaultAsync(d => d.Name == dish.Name && d.Category.Id == dish.CategoryId, cancellationToken)
                .ConfigureAwait(false);

            if (currentDish != null)
            {
                return this.Conflict("Такое блюдо в данной категории уже существует!");
            }
            
            var currentCategory = await this.db
                .Categories
                .FindAsync(dish.CategoryId)
                .ConfigureAwait(false);

            if (currentCategory == null)
            {
                return this.BadRequest("Категория не найдена!");
            }

            var mappedDish = this.mapper.Map<Dish>(dish);
            mappedDish.Category = currentCategory;
            var addedDish = await this.db.AddAsync(mappedDish, cancellationToken)
                .ConfigureAwait(false);
            var result = await this.db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return result > 0
                ? CreatedAtAction(nameof(this.Get), new { id = addedDish.Entity.Id }, new { id = addedDish.Entity.Id })
                : throw new InvalidOperationException("Не удалось добавить блюдо!");
        }

        /// <summary>
        /// Смена состояния блюда на готовое
        /// </summary>
        /// <param name="id">The id of dish.</param>
        /// <param name="dishState">The state of dish.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>Order.</returns>
        [HttpPost("SetState")]
#if !DEBUG
        [Authorize(Roles = "Admin, Bartender, Cook, Waiter")]
#endif
        public async Task<ActionResult<OrderViewModel>> SetState(int id, DishState dishState, CancellationToken ct)
        {
            var dish = await this.db.CookingDishes.FindAsync(id).ConfigureAwait(false);
            if (dish == null)
            {
                return this.NotFound("Блюдо не найдено!");
            }

            dish.DishState = dishState;
            var result = await db.SaveChangesAsync(ct).ConfigureAwait(false);
            return result > 0
                ? Ok(this.mapper.Map<OrderViewModel>(dish))
                : throw new InvalidOperationException("Не удалось изменить состояние блюда!");
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
            if (!string.Equals("d3a71c3d-abd2-4833-9686-e5c8818c9054", token,
                StringComparison.InvariantCultureIgnoreCase))
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
                var category =
                    categories.Single(c => c.ExternalId == dish.GroupID || c.Code == dish.GroupID.ToString());

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