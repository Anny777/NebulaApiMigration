namespace NebulaApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Dishes", "SubCategory_Id", "dbo.SubCategories");
            DropIndex("dbo.Dishes", new[] { "SubCategory_Id" });
            AddColumn("dbo.Categories", "WorkshopType", c => c.Int(nullable: false));
            AddColumn("dbo.Categories", "ExternalId", c => c.Int(nullable: false));
            AddColumn("dbo.Dishes", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Dishes", "ExternalId", c => c.Int(nullable: false));
            AddColumn("dbo.Dishes", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Dishes", "CreatedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Categories", "UrlImage");
            DropColumn("dbo.Dishes", "WorkshopType");
            DropColumn("dbo.Dishes", "_id");
            DropColumn("dbo.Dishes", "extId");
            DropColumn("dbo.Dishes", "shopID");
            DropColumn("dbo.Dishes", "barcode");
            DropColumn("dbo.Dishes", "nameFull");
            DropColumn("dbo.Dishes", "VAT");
            DropColumn("dbo.Dishes", "sellingPrice");
            DropColumn("dbo.Dishes", "useGroupMarginRule");
            DropColumn("dbo.Dishes", "isAlcohol");
            DropColumn("dbo.Dishes", "alcoholCode");
            DropColumn("dbo.Dishes", "ownMarginRule");
            DropColumn("dbo.Dishes", "modified");
            DropColumn("dbo.Dishes", "__v");
            DropColumn("dbo.Dishes", "isService");
            DropColumn("dbo.Dishes", "uuid");
            DropColumn("dbo.Dishes", "isDelete");
            DropColumn("dbo.Dishes", "code");
            DropColumn("dbo.Dishes", "isBeer");
            DropColumn("dbo.Dishes", "SubCategory_Id");
            DropTable("dbo.SubCategories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SubCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Dishes", "SubCategory_Id", c => c.Int());
            AddColumn("dbo.Dishes", "isBeer", c => c.Boolean(nullable: false));
            AddColumn("dbo.Dishes", "code", c => c.String());
            AddColumn("dbo.Dishes", "isDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Dishes", "uuid", c => c.String());
            AddColumn("dbo.Dishes", "isService", c => c.Boolean(nullable: false));
            AddColumn("dbo.Dishes", "__v", c => c.Int(nullable: false));
            AddColumn("dbo.Dishes", "modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Dishes", "ownMarginRule", c => c.String());
            AddColumn("dbo.Dishes", "alcoholCode", c => c.String());
            AddColumn("dbo.Dishes", "isAlcohol", c => c.Boolean(nullable: false));
            AddColumn("dbo.Dishes", "useGroupMarginRule", c => c.Boolean(nullable: false));
            AddColumn("dbo.Dishes", "sellingPrice", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Dishes", "VAT", c => c.String());
            AddColumn("dbo.Dishes", "nameFull", c => c.String());
            AddColumn("dbo.Dishes", "barcode", c => c.String());
            AddColumn("dbo.Dishes", "shopID", c => c.String());
            AddColumn("dbo.Dishes", "extId", c => c.String());
            AddColumn("dbo.Dishes", "_id", c => c.String());
            AddColumn("dbo.Dishes", "WorkshopType", c => c.Int(nullable: false));
            AddColumn("dbo.Categories", "UrlImage", c => c.String());
            DropColumn("dbo.Dishes", "CreatedDate");
            DropColumn("dbo.Dishes", "IsActive");
            DropColumn("dbo.Dishes", "ExternalId");
            DropColumn("dbo.Dishes", "Price");
            DropColumn("dbo.Categories", "ExternalId");
            DropColumn("dbo.Categories", "WorkshopType");
            CreateIndex("dbo.Dishes", "SubCategory_Id");
            AddForeignKey("dbo.Dishes", "SubCategory_Id", "dbo.SubCategories", "Id");
        }
    }
}
