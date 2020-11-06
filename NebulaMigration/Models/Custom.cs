namespace NebulaMigration.Models
{
    using NebulaMigration.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Custom
    {
        public Custom()
        {

        }

        public Custom(
            bool isActive,
            bool isOpened,
            int tableNumber,
            string comment)
        {
            this.IsActive = isActive;
            this.CreatedDate = DateTime.Now;
            this.CookingDishes = new List<CookingDish>();
            this.IsOpened = isOpened;
            this.TableNumber = tableNumber;
            this.Comment = comment;
        }

        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<CookingDish> CookingDishes { get; set; }
        public bool IsOpened { get; set; }
        public User User { get; set; }
        public bool IsExportRequested { get; set; }
        public int TableNumber { get; set; }
        public string Comment { get; set; }

        public void CloseOrder()
        {
            this.IsOpened = false;
        }

        public void SetStatusExportRequested(bool status)
        {
            this.IsExportRequested = status;
        }

        public void SetComment(string comment)
        {
            this.Comment += $" {comment}";
        }

        public OrderViewModel ToViewModel()
        {
            return new OrderViewModel()
            {
                Id = Id,
                Table = TableNumber,
                Dishes = CookingDishes.Select(c => c.ToViewModel()),
                CreatedDate = CreatedDate,
                Comment = Comment,
                IsExportRequested = IsExportRequested,
            };
        }
    }
}