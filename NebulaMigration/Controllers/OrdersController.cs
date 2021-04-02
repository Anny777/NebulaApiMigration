namespace NebulaMigration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Enums;
    using NebulaMigration;
    using ViewModels;

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
                .ThenInclude(c => c.Dish)
                .Where(c => c.IsOpened)
                .Select(c => this.mapper.Map<OrderViewModel>(c))
                .ToListAsync(ct)
                .ConfigureAwait(false);

            return this.Ok(orders);
        }

        /// <summary>
        /// Posts the specified order.
        /// </summary>
        /// <param name="tableNumber">Table number.</param>
        /// <param name="ct">The ct.</param>
        /// <returns>Order view model.</returns>
        [HttpPost]
        [Authorize(Roles = "Waiter, Admin")]
        public async Task<IActionResult> Post(int tableNumber, CancellationToken ct)
        {
            var custom = new Custom
            {
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                IsOpened = true,
                TableNumber = tableNumber,
            };

            await this.db.Customs.AddAsync(custom, ct).ConfigureAwait(false);
            var saveResult = await this.db.SaveChangesAsync(ct).ConfigureAwait(false);
            return saveResult > 0
                ? this.CreatedAtAction(nameof(this.Get), new { custom.Id }, new { custom.Id })
                : (IActionResult) this.StatusCode((int) HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Закрытие заказа 
        /// </summary>
        /// <param name="tableNumber">номер стола</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns></returns>
        [HttpPatch]
        [Authorize(Roles = "Bartender, Admin")]
        public async Task<ActionResult> Close(Guid id, CancellationToken ct)
        {
            var order = await this.db.Customs
                .FirstOrDefaultAsync(c => c.Id == id && c.IsOpened && c.IsActive, ct)
                .ConfigureAwait(false);

            this.CloseOrder(order);
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
        /// <param name="id">Идентификатор заказа.</param>
        /// <param name="comment">Комментарий к заказу.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns></returns>
        [HttpPatch("AddComment")]
        [Authorize(Roles = "Waiter, Bartender, Cook, Admin")]
        public async Task<ActionResult> AddComment(Guid id, string comment, CancellationToken ct)
        {
            try
            {
                var o = await this.db.Customs.FindAsync(id, ct).ConfigureAwait(false);
                if (o != null)
                {
                    o.Comment += Environment.NewLine + comment;
                    var saveResult = await this.db.SaveChangesAsync(ct).ConfigureAwait(false);
                    if (saveResult > 0)
                    {
                        return this.Ok();
                    }
                }

                return this.NotFound("Заказ не найден.");
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
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
            return this.Ok();
        }

        /// <summary>
        /// Gets the export orders.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Array of orders.</returns>
        [HttpGet("GetExportOrders")]
        public ActionResult<ExportOrder[]> GetExportOrders(string token)
        {
            if (!string.Equals("d3a71c3d-abd2-4833-9686-e5c8818c9054", token,
                StringComparison.InvariantCultureIgnoreCase))
            {
                return this.BadRequest("Доступ запрещен");
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

            return this.Ok(result);
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
            if (!string.Equals("d3a71c3d-abd2-4833-9686-e5c8818c9054", token,
                System.StringComparison.InvariantCultureIgnoreCase))
                return this.BadRequest("Доступ запрещен");

            this.db.Customs
                .Where(c => c.IsExportRequested && HandledOrders.Contains(c.TableNumber.ToString()))
                .ToList()
                .ForEach(c => c.IsExportRequested = false);
            this.db.SaveChanges();

            return this.Ok();
        }

        private void CloseOrder(Custom custom)
        {
            custom.IsOpened = false;
            custom.CookingDishes.ToList().ForEach(cd => cd.IsActive = false);
            custom.IsActive = false;
        }
    }
}