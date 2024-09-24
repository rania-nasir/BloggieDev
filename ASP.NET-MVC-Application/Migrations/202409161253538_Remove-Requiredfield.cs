namespace ASP.NET_MVC_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredfield : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "Author", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "Author", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
