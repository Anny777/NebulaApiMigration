using NebulaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace NebulaApi.Services
{
    public class Formula360
    {
//        public void SynchronizeDishes()
//        {
//            string data;
//#if DEBUG
//            data = System.IO.File.ReadAllText(@"C:\Users\bystrova-a\Desktop\response-data-export.json");
//#else
//            data = Formula360Connection.Get("https://lk.formula360.ru/services/remains/0/10000");
//#endif
//            var db = new ApplicationDbContext();

//            var extDishes = new JavaScriptSerializer().Deserialize<DishBaseObjects[]>(data);
//            var extDishesIds = extDishes.Select(c => c._id);
//            var currDishes = db.Dishes.Where(c => extDishesIds.Contains(c._id)).ToArray();
//            var newDishes = new ConcurrentBag<Dish>();
//            var category = db.Categories.Find(13);
//            var subCategory = db.SubCategories.Find(15);

//            extDishes.AsParallel().ForAll(dish =>
//            {
//                var currDish = currDishes.FirstOrDefault(c => c._id == dish._id);
//                if (currDish == null)
//                {
//                    var newDish = new Dish
//                    {
//                        Category = category,
//                        SubCategory = subCategory,
//                        IsAvailable = true
//                    };
//                    newDishes.Add(dish.MapTo(newDish));
//                }
//                else
//                {
//                    dish.MapTo(currDish);
//                }
//            });

//            if (newDishes.Any())
//            {
//                db.Dishes.AddRange(newDishes);
//            }

//            db.SaveChanges();
//        }
//    }

//    public static class Formula360Connection
//    {
//        public static string Token { get; private set; }

//        static Formula360Connection()
//        {
//            Token = "8fab906f586195b22fde47ba21baf50ff1123570f2a370bab41dab103b850698";

//        }

//        public static void SetToken(string token)
//        {
//            Token = token;
//        }

//        public static string Get(string url)
//        {
//            //var proxy = new WebProxy("mwg.corp.ingos.ru:9090", false)
//            //{
//            //    UseDefaultCredentials = true
//            //};

//            HttpClient client = null;
//            var httpClientHandler = new HttpClientHandler()
//            {
//                //Proxy = proxy,
//                PreAuthenticate = true,
//                UseDefaultCredentials = true,
//            };

//            client = new HttpClient(httpClientHandler);
//            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
//            var response = client.GetStringAsync(new Uri(url));
//            response.Wait();
//            return response.Result;
//        }
     }
}