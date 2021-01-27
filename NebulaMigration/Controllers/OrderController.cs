namespace NebulaApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using NebulaApi.Models;
    using NebulaMigration;
    using NebulaMigration.Models;
    using NebulaMigration.Models.Enums;
    using NebulaMigration.ViewModels;

    /// <summary>
    /// Order controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public partial class OrderController : ControllerBase
    {
        private readonly ApplicationContext db;
        private readonly IMapper mapper;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="db">Db.</param>
        /// <param name="mapper">Mapper.</param>
        public OrderController(ApplicationContext db, IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Получение открытых заказов (официант, кухня и бар будут брать блюда отсюда)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Waiter, Bartender, Cook, Admin")]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> Get(CancellationToken ct)
        {
            var orders = await this.db
                .Customs
                .Include(i => i.CookingDishes)
                .Where(c => c.IsOpened)
                .ToListAsync(ct)
                .ConfigureAwait(false);

            return this.Ok(orders.Select(this.mapper.Map<OrderViewModel>));
        }

        /// <summary>
        /// Получение заказа по номеру стола
        /// </summary>
        /// <param name="table">номер стола</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns></returns>
        [HttpGet("{table:int}")]
        [Authorize(Roles = "Waiter, Bartender, Cook, Admin")]
        public async Task<ActionResult<Custom>> Get(int table, CancellationToken ct)
        {
            var order = await this.db.Customs
                .FirstOrDefaultAsync(o => o.IsActive && o.IsOpened && o.TableNumber == table, ct)
                .ConfigureAwait(false);
            if (order == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.mapper.Map<OrderViewModel>(order));
        }

        /// <summary>
        /// Posts the specified order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="ct">The ct.</param>
        /// <returns>Order view model.</returns>
        [HttpPost]
        //[Authorize(Roles = "Waiter, Admin")]
        public async Task<ActionResult<OrderViewModel>> Post(OrderViewModel order, CancellationToken ct)
        {
            if (order == null || order.Dishes == null)
            {
                return this.BadRequest("Не получены необходимые данные");
            }

            var dishesId = order.Dishes.Select(c => c.Id).ToArray();
            var dishes = this.db.Dishes.Where(a => dishesId.Contains(a.Id));
            var custom = new Custom
            {
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CookingDishes = dishes.Select(this.mapper.Map<CookingDish>).ToArray(),
                IsOpened = true,
                TableNumber = order.Table,
                Comment = order.Comment,
            };

            this.db.Customs.Add(custom);
            var saveResult = await db.SaveChangesAsync(ct).ConfigureAwait(false);
            return Ok();
        }

        /// <summary>
        /// Добавление блюда к заказу
        /// </summary>
        /// <param name="dish">объект блюда</param>
        /// <param name="idOrder">идентификатор заказа</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>Order view model.</returns>
        [HttpPost("AddDishToOrder")]
        [Authorize(Roles = "Admin, Bartender, Waiter")]
        public async Task<ActionResult<OrderViewModel>> Post(DishViewModel dish, int idOrder, CancellationToken ct)
        {
            try
            {
                var order = await db.Customs.FindAsync(idOrder).ConfigureAwait(false);
                if (order == null)
                {
                    return this.NotFound("Заказ не найден.");
                }

                var currentDish = await db.Dishes.FindAsync(dish.Id).ConfigureAwait(false);
                if (currentDish == null)
                {
                    return this.NotFound("Блюдо не найдено.");
                }

                var newDish = new CookingDish
                {
                    IsActive = true,
                    Dish = currentDish,
                    DishState = DishState.InWork,
                    Comment = dish.Comment,
                };
                order.CookingDishes.Add(newDish);
                var result = await db.SaveChangesAsync(ct).ConfigureAwait(false);
                return result > 0
                    ? Ok(this.mapper.Map<OrderViewModel>(order))
                    : throw new InvalidOperationException("Не удалось добавить блюдо к заказу");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Закрытие заказа 
        /// </summary>
        /// <param name="tableNumber">номер стола</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns></returns>
        [HttpPatch]
        [Authorize(Roles = "Bartender, Admin")]
        public async Task<ActionResult> Close(int tableNumber, CancellationToken ct)
        {
            var customs = await this.db.Customs
                .Where(c => c.TableNumber == tableNumber && c.IsOpened && c.IsActive)
                .ToListAsync(ct)
                .ConfigureAwait(false);

            customs.ForEach(CloseOrder);
            var saveResult = await this.db.SaveChangesAsync(ct).ConfigureAwait(false);
            if (saveResult > 0)
            {
                return this.Ok();
            }

            return this.NotFound();
        }

        /// <summary>
        /// Добавление комментария к заказу
        /// </summary>
        /// <param name="order"></param>
        /// <param name="ct">Cancellatin token.</param>
        /// <returns></returns>
        [HttpPatch("AddComment")]
        [Authorize(Roles = "Waiter, Bartender, Cook, Admin")]
        public async Task<ActionResult> AddComment(OrderViewModel order, CancellationToken ct)
        {
            try
            {
                var o = await this.db.Customs.FindAsync(order.Id, ct).ConfigureAwait(false);
                if (o != null)
                {
                    o.Comment += order.Comment;
                    var saveResult = await db.SaveChangesAsync(ct).ConfigureAwait(false);
                    if (saveResult > 0)
                    {
                        return this.Ok();
                    }
                }

                return this.NotFound("Заказ не найден.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Проставление флага заказу для его синхронизации
        /// </summary>
        /// <param name="tableNumber"></param>
        /// <returns></returns>
        [HttpPost("SetExportOrder")]
        [Authorize(Roles = "Bartender, Admin")]
        public ActionResult SetExportOrder(int tableNumber)
        {
            // Необходимо доработать, тк есть связь с пользователем.

            //var db = new ApplicationDbContext();
            //var u = db.Users.Find(User.Identity.GetUserId());

            //db.Customs.Where(c => c.IsActive && c.IsOpened && c.TableNumber == tableNumber).ToList().ForEach(c =>
            //{
            //    c.IsExportRequested = true;
            //    c.User = u;
            //});

            //db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Gets the export orders.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Array of orders.</returns>
        [HttpGet("GetExportOrders")]
        public ActionResult<ExportOrder[]> GetExportOrders(string token)
        {
            if (!string.Equals("d3a71c3d-abd2-4833-9686-e5c8818c9054", token, StringComparison.InvariantCultureIgnoreCase))
            {
                return BadRequest("Доступ запрещен");
            }

            var result = this.db.Customs
             .Where(c => c.IsActive && c.IsExportRequested && c.IsOpened)
             .ToArray()
             .Select(c => new ExportOrder
             {
                 TableNumber = c.TableNumber.ToString(),
                 OperatorId = c.User.OperatorId,
                 Dishes = c.CookingDishes
                 .Where(d => d.IsActive && d.DishState == DishState.Taken)
                 .GroupBy(d => d.Dish.ExternalId)
                 .Select(d => new ExportDish
                 {
                     GoodId = d.Key,
                     Quantity = d.Count(),
                 }).ToArray()
             })
             .ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Sets the exported orders.
        /// </summary>
        /// <param name="HandledOrders">The handled orders.</param>
        /// <param name="token">The token.</param>
        /// <returns>Action result.</returns>
        [HttpPost("SetExportedOrders")]
        public ActionResult SetExportedOrders(string[] HandledOrders, string token)
        {
            if (!string.Equals("d3a71c3d-abd2-4833-9686-e5c8818c9054", token, System.StringComparison.InvariantCultureIgnoreCase))
                return BadRequest("Доступ запрещен");


            this.db.Customs
                .Where(c => c.IsExportRequested && HandledOrders.Contains(c.TableNumber.ToString()))
                .ToList()
                .ForEach(c => c.IsExportRequested = false);
            this.db.SaveChanges();

            return Ok();
        }

        private void CloseOrder(Custom custom)
        {
            custom.IsOpened = false;
            custom.CookingDishes.ToList().ForEach(cd => cd.IsActive = false);
            custom.IsActive = false;
        }
    }
}
