namespace NebulaApi.Models
{
    public class Order
    {
        public string TableNumber { get; set; }
        public int OperatorId { get; set; }
        public Dish[] Dishes { get; set; }
        public class Dish
        {
            public int GoodId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
