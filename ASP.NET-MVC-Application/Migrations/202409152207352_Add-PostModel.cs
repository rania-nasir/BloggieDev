namespace ASP.NET_MVC_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Heading = c.String(nullable: false, maxLength: 100),
                        PageTitle = c.String(nullable: false, maxLength: 200),
                        Content = c.String(nullable: false),
                        ShortDescription = c.String(maxLength: 300),
                        FeaturedImageUrl = c.String(),
                        UrlHandle = c.String(nullable: false, maxLength: 100),
                        PublishedDate = c.DateTime(nullable: false),
                        Author = c.String(nullable: false, maxLength: 50),
                        Visible = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PostTags",
                c => new
                    {
                        PostId = c.Guid(nullable: false),
                        TagId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.PostId, t.TagId })
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.TagId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostTags", "TagId", "dbo.Tags");
            DropForeignKey("dbo.PostTags", "PostId", "dbo.Posts");
            DropIndex("dbo.PostTags", new[] { "TagId" });
            DropIndex("dbo.PostTags", new[] { "PostId" });
            DropTable("dbo.PostTags");
            DropTable("dbo.Posts");
        }
    }
}
