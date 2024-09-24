namespace ASP.NET_MVC_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNotMapped : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostViewModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Heading = c.String(),
                        PageTitle = c.String(),
                        Content = c.String(),
                        ShortDescription = c.String(),
                        UrlHandle = c.String(),
                        PublishedDate = c.DateTime(nullable: false),
                        Author = c.String(),
                        Visible = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PostViewModels");
        }
    }
}
