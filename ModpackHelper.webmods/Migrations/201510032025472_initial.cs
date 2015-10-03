namespace ModpackHelper.webmods.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Mods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Modid = c.String(nullable: false, unicode: false),
                        Name = c.String(nullable: false, unicode: false),
                        Version = c.String(nullable: false, unicode: false),
                        Mcversion = c.String(nullable: false, unicode: false),
                        HelperUserId = c.Int(nullable: false),
                        JarMd5 = c.String(nullable: false, unicode: false),
                        Filename = c.String(nullable: false, unicode: false),
                        Url = c.String(unicode: false),
                        Status = c.Int(nullable: false),
                        AcceptedById = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AcceptedById)
                .ForeignKey("dbo.HelperUsers", t => t.HelperUserId, cascadeDelete: true)
                .Index(t => t.HelperUserId)
                .Index(t => t.AcceptedById);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(unicode: false),
                        Password = c.String(unicode: false),
                        AccessLevel = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HelperUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ip = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ModsByAuthors",
                c => new
                    {
                        AuthorRefId = c.Int(nullable: false),
                        ModRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AuthorRefId, t.ModRefId })
                .ForeignKey("dbo.Mods", t => t.AuthorRefId, cascadeDelete: true)
                .ForeignKey("dbo.Authors", t => t.ModRefId, cascadeDelete: true)
                .Index(t => t.AuthorRefId)
                .Index(t => t.ModRefId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mods", "HelperUserId", "dbo.HelperUsers");
            DropForeignKey("dbo.ModsByAuthors", "ModRefId", "dbo.Authors");
            DropForeignKey("dbo.ModsByAuthors", "AuthorRefId", "dbo.Mods");
            DropForeignKey("dbo.Mods", "AcceptedById", "dbo.Users");
            DropIndex("dbo.ModsByAuthors", new[] { "ModRefId" });
            DropIndex("dbo.ModsByAuthors", new[] { "AuthorRefId" });
            DropIndex("dbo.Mods", new[] { "AcceptedById" });
            DropIndex("dbo.Mods", new[] { "HelperUserId" });
            DropTable("dbo.ModsByAuthors");
            DropTable("dbo.HelperUsers");
            DropTable("dbo.Users");
            DropTable("dbo.Mods");
            DropTable("dbo.Authors");
        }
    }
}
