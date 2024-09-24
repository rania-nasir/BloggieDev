namespace ASP.NET_MVC_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletingTags : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Tags");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        DisplayName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
