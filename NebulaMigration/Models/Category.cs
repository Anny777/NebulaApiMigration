namespace NebulaMigration.Models
{
    using System;
    using NebulaMigration.Models.Enums;

    public class Category
    {
        public Category()
        {

        }

        public Category(
            bool isActive,
            string name,
            string code,
            WorkshopType workshopType,
            int externalId)
        {
            this.IsActive = isActive;
            this.CreatedDate = DateTime.Now;
            this.Name = name;
            this.Code = code;
            this.WorkshopType = workshopType;
            this.ExternalId = externalId;
        }
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public WorkshopType WorkshopType { get; set; }

        public void SetActive(bool isActive)
        {
            this.IsActive = isActive;
        }

        public void ChangeCategory(GoodsGroup goodsGroup)
        {
            this.ExternalId = goodsGroup.ID;
            this.Name = goodsGroup.Name;
            this.Code = goodsGroup.Code;
            this.SetActive(true);
        }
    }
}