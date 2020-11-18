namespace NebulaMigration.Models
{
    using System;
    using NebulaMigration.Models.Enums;

    public class Category
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public WorkshopType WorkshopType { get; set; }
    }
}