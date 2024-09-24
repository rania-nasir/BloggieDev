namespace ASP.NET_MVC_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredTechStackName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TechStackViewModels", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TechStackViewModels", "Name", c => c.String());
        }
    }
}
