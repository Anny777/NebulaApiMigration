namespace NebulaMigration.Models
{
    using System;

    public class Dish
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public string Consist { get; set; }
        public string Unit { get; set; }
        public bool IsAvailable { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}