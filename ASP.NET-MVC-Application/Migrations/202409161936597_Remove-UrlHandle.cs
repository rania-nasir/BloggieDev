namespace ASP.NET_MVC_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUrlHandle : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Posts", "UrlHandle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "UrlHandle", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
