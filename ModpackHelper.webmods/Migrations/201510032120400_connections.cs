namespace ModpackHelper.webmods.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class connections : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Connections",
                c => new
                    {
                        ConnectionId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ConnectionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Connections", "UserId", "dbo.Users");
            DropIndex("dbo.Connections", new[] { "UserId" });
            DropTable("dbo.Connections");
        }
    }
}
