namespace NebulaApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _ : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Categories", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CookingDishes", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.CookingDishes", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Customs", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Customs", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SubCategories", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.SubCategories", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubCategories", "CreatedDate");
            DropColumn("dbo.SubCategories", "IsActive");
            DropColumn("dbo.Customs", "CreatedDate");
            DropColumn("dbo.Customs", "IsActive");
            DropColumn("dbo.CookingDishes", "CreatedDate");
            DropColumn("dbo.CookingDishes", "IsActive");
            DropColumn("dbo.Categories", "CreatedDate");
            DropColumn("dbo.Categories", "IsActive");
        }
    }
}
