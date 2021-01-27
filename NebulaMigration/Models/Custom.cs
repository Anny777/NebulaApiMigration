namespace NebulaMigration.Models
{
    using System;
    using System.Collections.Generic;

    public class Custom
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<CookingDish> CookingDishes { get; set; }
        public bool IsOpened { get; set; }
        public User User { get; set; }
        public bool IsExportRequested { get; set; }
        public int TableNumber { get; set; }
        public string Comment { get; set; }
    }
}