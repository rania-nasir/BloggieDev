namespace ASP.NET_MVC_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveViewmodetechstack : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TechStacks", "Name", c => c.String(nullable: false));
            DropTable("dbo.TechStackViewModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TechStackViewModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.TechStacks", "Name", c => c.String());
        }
    }
}
