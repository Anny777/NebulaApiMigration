namespace NebulaApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customs", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Customs", "User_Id");
            AddForeignKey("dbo.Customs", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customs", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Customs", new[] { "User_Id" });
            DropColumn("dbo.Customs", "User_Id");
        }
    }
}
