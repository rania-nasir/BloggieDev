namespace ASP.NET_MVC_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostTechStack : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostTechStacks",
                c => new
                    {
                        PostId = c.Guid(nullable: false),
                        TechStackId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.PostId, t.TechStackId })
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.TechStacks", t => t.TechStackId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.TechStackId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostTechStacks", "TechStackId", "dbo.TechStacks");
            DropForeignKey("dbo.PostTechStacks", "PostId", "dbo.Posts");
            DropIndex("dbo.PostTechStacks", new[] { "TechStackId" });
            DropIndex("dbo.PostTechStacks", new[] { "PostId" });
            DropTable("dbo.PostTechStacks");
        }
    }
}
