namespace ASP.NET_MVC_Application.Migrations
{
    using ASP.NET_MVC_Application.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ASP.NET_MVC_Application.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ASP.NET_MVC_Application.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            // Seed data for roles and users
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // Create SuperAdmin role if it does not exist and assign the role by hardcoded
            if (!roleManager.RoleExists("SuperAdmin"))
            {
                var role = new IdentityRole { Name = "SuperAdmin" };
                roleManager.Create(role);

                var superAdminUser = new ApplicationUser
                {
                    UserName = "rania.nasir@devsinc.com",
                    Email = "rania.nasir@devsinc.com",
                    FirstName = "Rania",
                    LastName = "Nasir"
                };

                string superAdminPassword = "Rania@123";
                var result = userManager.Create(superAdminUser, superAdminPassword);

                if (result.Succeeded)
                {
                    userManager.AddToRole(superAdminUser.Id, "SuperAdmin");
                }
            }

            // Create User role if it does not exist
            if (!roleManager.RoleExists("User"))
            {
                roleManager.Create(new IdentityRole("User"));
            }

            // Create Admin role if it does not exist
            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(new IdentityRole("Admin"));
            }

        }
    }
}
