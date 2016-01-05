namespace WebShop.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePhotoTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhotoGood",
                c => new
                    {
                        PhotoId = c.Int(nullable: false, identity: true),
                        PhotoName = c.String(),
                        MimeType = c.String(),
                        Photo = c.Binary(),
                        GoodId = c.Int(),
                    })
                .PrimaryKey(t => t.PhotoId)
                .ForeignKey("dbo.Good", t => t.GoodId, cascadeDelete: true)
                .Index(t => t.GoodId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PhotoGood", "GoodId", "dbo.Good");
            DropIndex("dbo.PhotoGood", new[] { "GoodId" });
            DropTable("dbo.PhotoGood");
        }
    }
}
