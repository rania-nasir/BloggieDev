﻿namespace ASP.NET_MVC_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeckStackDbContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TechStacks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TechStacks");
        }
    }
}