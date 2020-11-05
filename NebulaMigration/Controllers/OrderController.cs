using Microsoft.AspNet.Identity;
using NebulaApi.Models;
using NebulaApi.ViewModels;
using ProjectOrderFood.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace NebulaApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("Order")]
    public class OrderController : ApiController
    {
        /// <summary>
        /// Получение заказа по номеру стола
        /// </summary>
        /// <param name="table">номер стола</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Waiter, Bartender, Cook, Admin")]
        [Route("GetOrder")]
        public IHttpActionResult GetOrder(int table)
        {
            var order = new ApplicationDbContext().Customs.FirstOrDefault(o => o.IsActive && o.IsOpened && o.TableNumber == table);

            if (order == null)
            {
                return Ok();
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
        [Route("New")]
        public IHttpActionResult New(OrderViewModel order)
        {
            if (order == null || order.Dishes == null)
            {
                return BadRequest("Не получены необходимые данные");
            }
            var db = new ApplicationDbContext();

            //// Новый заказ
            //if (order.Id <= 0)
            //{
            //var od = order.Dishes.Select(d => d.Id).Distinct().ToArray();
            //var dishes = db.Dishes.Where(c => od.Contains(c.Id)).ToList();
            var o = new Custom()
            {
                IsOpened = true,
                TableNumber = order.Table,
                CookingDishes = new List<CookingDish>(),
                Comment = order.Comment
            };
            db.Customs.Add(o);
            //}
            //else
            //{
            //    var newDishes = order.Dishes.Where(d => d.CookingDishId <= 0).ToArray();
            //    var od = newDishes.Select(d => d.Id).Distinct().ToArray();
            //    var custom = db.Customs.Find(order.Id);

            //    if (od.Count() > 0)
            //    {
            //        var dishes = db.Dishes.Where(c => od.Contains(c.Id)).ToArray();


            //        if (custom == null)
            //        {
            //            return BadRequest("Не найден заказ");
            //        }
            //        newDishes.Select(a => new CookingDish()
            //        {
            //            Comment = a.Comment,
            //            Dish = dishes.Single(c => c.Id == a.Id),
            //            DishState = DishState.InWork,
            //            CreatedDate = a.CreatedDate
            //        }).ToList().ForEach(custom.CookingDishes.Add);
            //    }

            //    // Запрос на удаление блюда из заказа (с помощью Except возвращает блюда, которым нужно сменить статус)
            //    var crd = custom.CookingDishes.Select(c => c.Id)
            //        .Except(order.Dishes.Where(c => c.CookingDishId > 0).Select(c => c.CookingDishId));

            //    db.CookingDishes.Where(c => crd.Contains(c.Id)).ToList()
            //        .ForEach(c => { c.DishState = DishState.CancellationRequested; });
            //}

            db.SaveChanges();
            return Ok(o.ToViewModel());
        }

        /// <summary>
        /// Получение открытых заказов (официант, кухня и бар будут брать блюда отсюда)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Waiter, Bartender, Cook, Admin")]
        [Route("List")]
        public IHttpActionResult List()
        {
            var db = new ApplicationDbContext();
            var orders = db.Customs.Where(c => c.IsOpened).ToList().Select(c => c.ToViewModel());
            return Ok(orders);
        }

        /// <summary>
        /// Закрытие заказа 
        /// </summary>
        /// <param name="tableNumber">номер стола</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Bartender, Admin")]
        [Route("Close")]
        public IHttpActionResult Close(int tableNumber)
        {
            var db = new ApplicationDbContext();
            db.Customs.Where(c => c.TableNumber == tableNumber && c.IsOpened && c.IsActive).ToList().ForEach(c => { c.IsOpened = false; });

            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Добавление комментария к заказу
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Waiter, Bartender, Cook, Admin")]
        [Route("AddComment")]
        public IHttpActionResult AddComment(OrderViewModel order)
        {
            try
            {
                var db = new ApplicationDbContext();
                var o = db.Customs.Find(order.Id);
                o.Comment = order.Comment;

                db.SaveChanges();

                return Ok(o.ToViewModel());
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Проставление флага заказу для его синхронизации
        /// </summary>
        /// <param name="tableNumber"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Bartender, Admin")]
        [Route("SetExportOrder")]
        public IHttpActionResult SetExportOrder(int tableNumber)
        {
            var db = new ApplicationDbContext();
            var u = db.Users.Find(User.Identity.GetUserId());

            db.Customs.Where(c => c.IsActive && c.IsOpened && c.TableNumber == tableNumber).ToList().ForEach(c =>
            {
                c.IsExportRequested = true;
                c.User = u;
            });

            db.SaveChanges();
            return Ok();
        }

        [Route("GetExportOrders")]
        public IHttpActionResult GetExportOrders(string token)
        {
            if (!string.Equals("d3a71c3d-abd2-4833-9686-e5c8818c9054", token, System.StringComparison.InvariantCultureIgnoreCase))
                return BadRequest("Доступ запрещен");

            var result = new ApplicationDbContext()
                .Customs
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

        [HttpPost]
        [Route("SetExportedOrders")]
        public IHttpActionResult SetExportedOrders(string[] HandledOrders, string token)
        {
            if (!string.Equals("d3a71c3d-abd2-4833-9686-e5c8818c9054", token, System.StringComparison.InvariantCultureIgnoreCase))
                return BadRequest("Доступ запрещен");

            using (var db = new ApplicationDbContext())
            {
                db
                    .Customs
                    .Where(c => c.IsExportRequested && HandledOrders.Contains(c.TableNumber.ToString()))
                    .ToList()
                    .ForEach(c => c.IsExportRequested = false);
                db.SaveChanges();
            }

            return Ok();
        }
    }
}
