using System.Linq;
using NebulaApi.Models;
using System.Web.Http;
using NebulaApi.ViewModels;
using System.Web.Http.Cors;
using ProjectOrderFood.Enums;
using NebulaSync.ExternalModels;

namespace NebulaApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("Dish")]
    public class DishController : ApiController
    {
        //public void SetToken(string token)
        //{
        //    Formula360Connection.SetToken(token);
        //}

        /// <summary>
        /// Получение списка блюд
        /// /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Waiter, Bartender, Cook, Admin")]
        [Route("List")]
        public IHttpActionResult List()
        {
            var db = new ApplicationDbContext();
            var response = db.Dishes.Select(c => new DishViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                Consist = c.Consist,
                Price = c.Price,
                Unit = c.Unit
            }).OrderBy(b => b.Name).ToList();
            return Json(response);
        }

        /// <summary>
        /// Смена состояния блюда на готовое
        /// </summary>
        /// <param name="id">идентификатор блюда</param>
        /// <param name="dishState">статус блюда</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin, Bartender, Cook, Waiter")]
        [Route("SetState")]
        public IHttpActionResult SetState(int id, DishState dishState)
        {
            try
            {
                var db = new ApplicationDbContext();
                var dish = db.CookingDishes.Find(id);
                if (dish == null)
                {
                    return BadRequest("Блюдо не найдено!");
                }
                dish.DishState = dishState;
                db.SaveChanges();
                return Ok(dish.Custom.ToViewModel());
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Добавление блюда
        /// </summary>
        /// <param name="dish">объект блюда</param>
        /// <param name="idOrder">идентификатор заказа</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin, Bartender, Waiter")]
        [Route("AddDish")]
        public IHttpActionResult AddDish(DishViewModel dish, int idOrder)
        {
            try
            {
                var db = new ApplicationDbContext();
                var order = db.Customs.Find(idOrder);
                var currentDish = db.Dishes.Find(dish.Id);

                var newDish = new CookingDish()
                {
                    Dish = currentDish,
                    DishState = DishState.InWork,
                    IsActive = true
                };
                order.CookingDishes.Add(newDish);
                db.SaveChanges();

                return Ok(order.ToViewModel());
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Sync")]
        public IHttpActionResult Sync(SyncModel data, string token)
        {
            if (!string.Equals("d3a71c3d-abd2-4833-9686-e5c8818c9054", token, System.StringComparison.InvariantCultureIgnoreCase))
                return BadRequest("Доступ запрещен");

            if (data == null)
                return BadRequest("Не переданы данные");

            if (data.Categories == null || data.Categories.Length == 0)
                return BadRequest("Передан пустой список категорий");

            if (data.Goods == null || data.Goods.Length == 0)
                return BadRequest("Передан пустой список блюд");

            var db = new ApplicationDbContext();
            db.Categories.AsParallel().ForAll(c => { c.IsActive = false; });
            foreach (var category in data.Categories)
            {
                var current = db.Categories.FirstOrDefault(c => c.ExternalId == category.ID);
                if (current == null)
                {
                    current = new Category()
                    {
                        WorkshopType = WorkshopType.Kitchen
                    };
                    db.Categories.Add(current);
                }

                current.ExternalId = category.ID;
                current.Name = category.Name;
                current.Code = category.Code;
                current.IsActive = true;
            }

            db.SaveChanges();
            db.Dishes.AsParallel().ForAll(c => { c.IsActive = false; });
            var categories = db.Categories.ToArray();
            foreach (var dish in data.Goods)
            {
                var current = db.Dishes.FirstOrDefault(c => c.ExternalId == dish.ID);
                if (current == null)
                {
                    current = new Dish();
                    db.Dishes.Add(current);
                }

                current.ExternalId = dish.ID;
                current.Category = categories.Single(c => c.ExternalId == dish.GroupID || c.Code == dish.GroupID.ToString());
                current.Name = dish.Name;
                current.Consist = dish.Description;
                current.IsAvailable = true;
                current.Price = dish.PriceOut2.HasValue ? decimal.Parse(dish.PriceOut2.Value.ToString()) : 0;
                current.Unit = dish.Measure1;
                current.IsActive = true;
            }

            db.SaveChanges();
            return Ok();
        }

        public class SyncModel
        {
            public Good[] Goods { get; set; }
            public GoodsGroup[] Categories { get; set; }
        }
    }
}
