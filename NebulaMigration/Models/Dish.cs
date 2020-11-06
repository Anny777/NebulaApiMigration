namespace NebulaMigration.Models
{
    using System;

    public class Dish
    {
        bool isActive;
        DateTime createdDate;
        string consist;
        string unit;
        bool isAvailable;
        string name;
        decimal price;
        Category category;
        int externalId;

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
            this.isActive = isActive;
            this.createdDate = DateTime.Now;
            this.consist = consist;
            this.unit = unit;
            this.isAvailable = isAvailable;
            this.name = name;
            this.price = price;
            this.category = category;
            this.externalId = externalId;
        }
        public Guid Id { get; set; }
        public int ExternalId => this.externalId;
        public bool IsActive => this.isActive;
        public DateTime CreatedDate => this.createdDate;
        public Category Category => this.category;
        public string Consist => this.consist;
        public string Unit => this.consist;
        public bool IsAvailable => this.isAvailable;
        public string Name => this.name;
        public decimal Price => this.price;

        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
        }

        public void ChangeDish(Good dish, Category category, decimal price)
        {
            this.externalId = dish.ID;
            this.category = category;
            this.name = dish.Name;
            this.consist = dish.Description;
            this.isAvailable = true;
            this.price = price;
            this.unit = dish.Measure1;
            this.SetActive(true);
        }
    }
}