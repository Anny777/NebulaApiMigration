namespace NebulaMigration.Models
{
    using NebulaMigration.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Custom
    {
        bool isActive;
        DateTime createdDate;
        List<CookingDish> cookingDishes;
        bool isOpened;
        User user;
        bool isExportRequested;
        int tableNumber;
        string comment;

        public Custom(
            bool isActive,
            List<CookingDish> cookingDishes,
            bool isOpened,
            int tableNumber,
            string comment)
        {
            this.isActive = isActive;
            this.createdDate = DateTime.Now;
            this.cookingDishes = cookingDishes;
            this.isOpened = isOpened;
            this.tableNumber = tableNumber;
            this.comment = comment;
        }

        public Guid Id { get; set; }
        public bool IsActive => this.isActive;
        public DateTime CreatedDate => this.createdDate;
        public ICollection<CookingDish> CookingDishes => this.cookingDishes;
        public bool IsOpened => this.isOpened;
        public User User => this.user;
        public bool IsExportRequested => this.isExportRequested;
        public int TableNumber => this.tableNumber;
        public string Comment => this.comment;

        public void CloseOrder()
        {
            this.isOpened = false;
        }

        public void SetStatusExportRequested(bool status)
        {
            this.isExportRequested = status;
        }

        public void SetComment(string comment)
        {
            this.comment += $" {comment}";
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