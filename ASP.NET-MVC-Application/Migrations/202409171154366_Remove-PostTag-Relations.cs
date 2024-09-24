namespace ASP.NET_MVC_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePostTagRelations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PostTags", "PostId", "dbo.Posts");
            DropForeignKey("dbo.PostTags", "TagId", "dbo.Tags");
            DropIndex("dbo.PostTags", new[] { "PostId" });
            DropIndex("dbo.PostTags", new[] { "TagId" });
            DropTable("dbo.PostTags");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PostTags",
                c => new
                    {
                        PostId = c.Guid(nullable: false),
                        TagId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.PostId, t.TagId });
            
            CreateIndex("dbo.PostTags", "TagId");
            CreateIndex("dbo.PostTags", "PostId");
            AddForeignKey("dbo.PostTags", "TagId", "dbo.Tags", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PostTags", "PostId", "dbo.Posts", "Id", cascadeDelete: true);
        }
    }
}
