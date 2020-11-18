using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NebulaMigration.Models;
using NebulaMigration.Models.Enums;
using NebulaMigration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NebulaMigration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly ApplicationContext db;

        public DishController(ApplicationContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        //public void SetToken(string token)
        //{
        //    Formula360Connection.SetToken(token);
        //}

        /// <summary>
        /// Получение списка блюд
        /// /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        //[Authorize(Roles = "Waiter, Bartender, Cook, Admin")]
        public ActionResult<List<DishViewModel>> Get()
        {
            return this.db.Dishes.Select(c => new DishViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                Consist = c.Consist,
                Price = c.Price,
                Unit = c.Unit
            }).OrderBy(b => b.Name).ToList();
        }

        /// <summary>
        /// Смена состояния блюда на готовое
        /// </summary>
        /// <param name="id">идентификатор блюда</param>
        /// <param name="dishState">статус блюда</param>
        /// <returns></returns>
        [HttpPost("SetState")]
        [Authorize(Roles = "Admin, Bartender, Cook, Waiter")]
        public ActionResult<OrderViewModel> SetState(int id, DishState dishState)
        {
            try
            {
                var dish = this.db.CookingDishes.Find(id);
                if (dish == null)
                {
                    return BadRequest("Блюдо не найдено!");
                }

                dish.SetState(dishState);
                db.SaveChanges();
                return Ok(dish.Custom.ToViewModel());
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
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles = "Admin, Bartender, Waiter")]
        public ActionResult<OrderViewModel> Post(DishViewModel dish, int idOrder)
        {
            try
            {
                var custom = db.Customs.Find(idOrder);
                var currentDish = db.Dishes.Find(dish.Id);

                var newDish = new CookingDish(true, currentDish, DishState.InWork, dish.Comment);
                custom.CookingDishes.ToList().Add(newDish);
                db.SaveChanges();
                return Ok(custom.ToViewModel());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Sync")]
        public ActionResult Sync(SyncModel data, string token)
        {
            if (!string.Equals("d3a71c3d-abd2-4833-9686-e5c8818c9054", token, StringComparison.InvariantCultureIgnoreCase))
                return BadRequest("Доступ запрещен");

            if (data == null)
                return BadRequest("Не переданы данные");

            if (data.Categories == null || data.Categories.Length == 0)
                return BadRequest("Передан пустой список категорий");

            if (data.Goods == null || data.Goods.Length == 0)
                return BadRequest("Передан пустой список блюд");

            this.db.Categories.AsParallel().ForAll(c => c.SetActive(false));
            foreach (var category in data.Categories)
            {
                var current = db.Categories.FirstOrDefault(c => c.ExternalId == category.ID);
                if (current == null)
                {
                    db.Categories.Add(new Category(true, category.Name, category.Code, WorkshopType.Kitchen, category.ID));
                    break;
                }

                current.ChangeCategory(category);
            }

            db.SaveChanges();

            db.Dishes.AsParallel().ForAll(c => c.SetActive(false));
            var categories = db.Categories.ToArray();
            foreach (var dish in data.Goods)
            {
                var price = dish.PriceOut2.HasValue ? decimal.Parse(dish.PriceOut2.Value.ToString()) : 0;
                var category = categories.Single(c => c.ExternalId == dish.GroupID || c.Code == dish.GroupID.ToString());

                var current = db.Dishes.FirstOrDefault(c => c.ExternalId == dish.ID);
                if (current == null)
                {
                    db.Dishes.Add(new Dish(true, dish.Description, dish.Measure1, true, dish.Name, price, category, dish.ID));
                    break;
                }

                current.ChangeDish(dish, category, price);
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
