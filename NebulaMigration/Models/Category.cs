namespace NebulaMigration.Models
{
    using System;
    using NebulaMigration.Models.Enums;

    public class Category
    {
        bool isActive;
        DateTime createdDate;
        string name;
        string code;
        WorkshopType workshopType;
        int externalId;

        public Category(
            bool isActive,
            string name,
            string code,
            WorkshopType workshopType,
            int externalId)
        {
            this.isActive = isActive;
            this.createdDate = DateTime.Now;
            this.name = name;
            this.code = code;
            this.workshopType = workshopType;
            this.externalId = externalId;
        }
        public Guid Id { get; set; }
        public int ExternalId => this.externalId;
        public bool IsActive => this.isActive;
        public DateTime CreatedDate => this.createdDate;
        public string Name => this.name;
        public string Code => this.code;
        public WorkshopType WorkshopType => this.workshopType;

        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
        }

        public void ChangeCategory(GoodsGroup goodsGroup) 
        {
            this.externalId = goodsGroup.ID;
            this.name = goodsGroup.Name;
            this.code = goodsGroup.Code;
            this.SetActive(true);
        }
    }
}