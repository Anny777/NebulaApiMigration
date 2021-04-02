namespace NebulaMigration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Extensions;
    using Microsoft.AspNetCore.Authorization;
    using ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Enums;

    /// <summary>
    /// Cooking dishes controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin, Bartender, Waiter")]
    public class CookingDishesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ApplicationContext db;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="mapper">Mapper.</param>
        /// <param name="db">Db context.</param>
        public CookingDishesController(
            IMapper mapper,
            ApplicationContext db)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Получение списка категорий.
        /// /// </summary>
        /// <returns>Список категорий блюд.</returns>
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CookingDishViewModel>>> Get(Guid orderId, CancellationToken ct)
        {
            var data = await this.db
                .CookingDishes
                .Where(c => c.CustomId == orderId)
                .Include(c => c.Dish)
                .OrderBy(b => b.Dish.Name)
                .Select(c => this.mapper.Map<CookingDishViewModel>(c))
                .ToArrayAsync(ct)
                .ConfigureAwait(false);

            return this.Ok(data);
        }

        /// <summary>
        /// Добавление блюда к заказу
        /// </summary>
        /// <param name="dishId">объект блюда</param>
        /// <param name="id">идентификатор заказа</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>Order view model.</returns>
        [HttpPost("AddDish")]
        public async Task<IActionResult> Post(Guid id, Guid dishId, CancellationToken ct)
        {
            try
            {
                var isOrderExists = await this.db
                    .Customs
                    .AnyAsync(c => c.Id == id && c.IsActive && c.IsOpened, ct)
                    .ConfigureAwait(false);
                if (!isOrderExists)
                {
                    return this.NotFound("Заказ не найден.");
                }

                var isDishExists = await this.db.Dishes
                    .AnyAsync(d => d.Id == dishId && d.IsActive && d.IsAvailable, ct)
                    .ConfigureAwait(false);
                if (!isDishExists)
                {
                    return this.NotFound("Блюдо не найдено.");
                }

                var newDish = new CookingDish
                {
                    IsActive = true,
                    DishId = dishId,
                    CustomId = id,
                    DishState = DishState.InWork,
                    CreatedDate = DateTime.UtcNow,
                };

                await db.CookingDishes.AddAsync(newDish, ct).ConfigureAwait(false);
                var result = await this.db.SaveChangesAsync(ct).ConfigureAwait(false);
                return result > 0
                    ? this.NoContent()
                    : throw new InvalidOperationException("Не удалось добавить блюдо к заказу");
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Смена состояния блюда на готовое.
        /// </summary>
        /// <param name="id">The id of dish.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>Order.</returns>
        [HttpPost("RequestCancellation")]
        [Authorize(Roles = "Admin, Bartender, Waiter")]
        public async Task<ActionResult> RequestCancellation(Guid id, CancellationToken ct)
        {
            return await this.SetState(id, DishState.CancellationRequested, DishState.CancellationRequested, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Смена состояния блюда на готовое.
        /// </summary>
        /// <param name="id">The id of dish.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>Order.</returns>
        [HttpPost("SetReady")]
        [Authorize(Roles = "Admin, Bartender, Cook")]
        public async Task<ActionResult> SetReady(Guid id, CancellationToken ct)
        {
            return await this.SetState(id, DishState.InWork, DishState.Ready, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Смена состояния блюда на забрано.
        /// </summary>
        /// <param name="id">The id of dish.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>Order.</returns>
        [HttpPost("SetTaken")]
        [Authorize(Roles = "Admin, Bartender, Waiter")]
        public async Task<ActionResult> SetTaken(Guid id, CancellationToken ct)
        {
            return await this.SetState(id, DishState.Ready, DishState.Taken, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Смена состояния блюда на удаленное.
        /// </summary>
        /// <param name="id">The id of dish.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>Order.</returns>
        [HttpPost("SetDeleted")]
        [Authorize(Roles = "Admin, Bartender, Waiter")]
        public async Task<ActionResult> SetDeleted(Guid id, CancellationToken ct)
        {
            return await this.SetState(id, DishState.CancellationRequested, DishState.Deleted, ct)
                .ConfigureAwait(false);
        }

        private async Task<ActionResult> SetState(Guid id, DishState oldState, DishState newState, CancellationToken ct)
        {
            var dish = await this.db.CookingDishes.FindAsync(id).ConfigureAwait(false);
            if (dish == null)
            {
                return this.NotFound("Блюдо не найдено!");
            }

            if (dish.DishState != oldState && oldState != newState)
            {
                return this.BadRequest(
                    $"Блюдо находится в состоянии {EnumHelper<DishState>.GetDisplayValue(dish.DishState)}, а должен быть {EnumHelper<DishState>.GetDisplayValue(oldState)}");
            }
            
            dish.DishState = newState;
            var result = await db.SaveChangesAsync(ct).ConfigureAwait(false);
            return result > 0
                ? Ok()
                : throw new InvalidOperationException("Не удалось изменить состояние блюда!");
        }
    }
}