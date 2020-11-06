namespace NebulaMigration.Models
{
    using System;

    public class Dish
    {
        public Dish()
        {

        }

        public Dish(
            bool isActive,
            string consist,
            string unit,
            bool isAvailable,
            string name,
            decimal price,
            Category category,
            int externalId)
        {
            this.IsActive = isActive;
            this.CreatedDate = DateTime.Now;
            this.Consist = consist;
            this.Unit = unit;
            this.IsAvailable = isAvailable;
            this.Name = name;
            this.Price = price;
            this.Category = category;
            this.ExternalId = externalId;
        }
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Category Category { get; set; }
        public string Consist { get; set; }
        public string Unit { get; set; }
        public bool IsAvailable { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public void SetActive(bool isActive)
        {
            this.IsActive = isActive;
        }

        public void ChangeDish(Good dish, Category category, decimal price)
        {
            this.ExternalId = dish.ID;
            this.Category = category;
            this.Name = dish.Name;
            this.Consist = dish.Description;
            this.IsAvailable = true;
            this.Price = price;
            this.Unit = dish.Measure1;
            this.SetActive(true);
        }
    }
}