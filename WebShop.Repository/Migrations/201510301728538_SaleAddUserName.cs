namespace WebShop.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaleAddUserName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sale", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sale", "UserName");
        }
    }
}
