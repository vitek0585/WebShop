namespace WebShop.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotoTwo : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PhotoGoods", newName: "PhotoGood");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.PhotoGood", newName: "PhotoGoods");
        }
    }
}
