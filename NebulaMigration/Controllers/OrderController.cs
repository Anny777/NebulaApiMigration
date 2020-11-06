

namespace NebulaApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using NebulaMigration;
    using NebulaMigration.Models;
    using NebulaMigration.Models.Enums;
    using NebulaMigration.ViewModels;

    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationContext db;

        public OrderController(ApplicationContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Получение заказа по номеру стола
        /// </summary>
        /// <param name="table">номер стола</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Waiter, Bartender, Cook, Admin")]
        public ActionResult<Custom> Get(int table)
        {
            var order = this.db.Customs.FirstOrDefault(o => o.IsActive && o.IsOpened && o.TableNumber == table);
            if (order == null)
            {
                return NotFound("Заказ не найден");
            }

            return Ok(order.ToViewModel());
        }

        /// <summary>
        /// Создание нового заказа
        /// </summary>
        /// <param name="order">заказ</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Waiter, Admin")]
        public ActionResult<OrderViewModel> Post(OrderViewModel order)
        {
            if (order == null || order.Dishes == null)
            {
                return this.BadRequest("Не получены необходимые данные");
            }

            this.db.Customs.Add(new Custom(true, true, order.Table, order.Comment));
            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Получение открытых заказов (официант, кухня и бар будут брать блюда отсюда)
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOpenedOrders")]
        [Authorize(Roles = "Waiter, Bartender, Cook, Admin")]
        public ActionResult<OrderViewModel[]> GetOpenedOrder()
        {
            var orders = this.db.Customs.Where(c => c.IsOpened).ToList().Select(c => c.ToViewModel());
            return Ok(orders);
        }

        /// <summary>
        /// Закрытие заказа 
        /// </summary>
        /// <param name="tableNumber">номер стола</param>
        /// <returns></returns>
        [HttpPatch]
        [Authorize(Roles = "Bartender, Admin")]
        public ActionResult Close(int tableNumber)
        {
            this.db.Customs
                .Where(c => c.TableNumber == tableNumber && c.IsOpened && c.IsActive)
                .ToList()
                .ForEach(c => c.CloseOrder());
            this.db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Добавление комментария к заказу
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPatch("AddComment")]
        [Authorize(Roles = "Waiter, Bartender, Cook, Admin")]
        public ActionResult AddComment(OrderViewModel order)
        {
            try
            {
                var o = this.db.Customs.Find(order.Id);
                if (o != null)
                {
                    o.SetComment(order.Comment);
                    db.SaveChanges();
                    return Ok();
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

        [HttpGet("GetExportOrders")]
        public ActionResult<Order[]> GetExportOrders(string token)
        {
            if (!string.Equals("d3a71c3d-abd2-4833-9686-e5c8818c9054", token, System.StringComparison.InvariantCultureIgnoreCase))
            {
                return BadRequest("Доступ запрещен");
            }

            var result = this.db.Customs
             .Where(c => c.IsActive && c.IsExportRequested && c.IsOpened)
             .ToArray()
             .Select(c => new Order
             {
                 TableNumber = c.TableNumber.ToString(),
                 OperatorId = c.User.OperatorId,
                 Dishes = c.CookingDishes
                 .Where(d => d.IsActive && d.DishState == DishState.Taken)
                 .GroupBy(d => d.Dish.ExternalId)
                 .Select(d => new Order.dish
                 {
                     GoodId = d.Key,
                     Quantity = d.Count()
                 }).ToArray()
             })
             .ToArray();

            return Ok(result);
        }

        public class Order
        {
            public string TableNumber { get; set; }
            public int OperatorId { get; set; }
            public dish[] Dishes { get; set; }
            public class dish
            {
                public int GoodId { get; set; }
                public int Quantity { get; set; }
            }
        }

        [HttpPost("SetExportedOrders")]
        public ActionResult SetExportedOrders(string[] HandledOrders, string token)
        {
            if (!string.Equals("d3a71c3d-abd2-4833-9686-e5c8818c9054", token, System.StringComparison.InvariantCultureIgnoreCase))
                return BadRequest("Доступ запрещен");


            this.db.Customs
                .Where(c => c.IsExportRequested && HandledOrders.Contains(c.TableNumber.ToString()))
                .ToList()
                .ForEach(c => c.SetStatusExportRequested(false));
            this.db.SaveChanges();

            return Ok();
        }
    }
}
