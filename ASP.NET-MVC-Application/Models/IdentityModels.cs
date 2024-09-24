using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ASP.NET_MVC_Application.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //DbSet for tag and post entity
        public DbSet<Post> Posts { get; set; }
        public DbSet<TechStack> TechStacks { get; set; }

        // Dbset for Post-TechStack entity
        public DbSet<PostTechStack> PostTechStacks { get; set; }

        // Built m-to-m relationship between Posts and TechStack
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostTechStack>()
                .HasKey(pt => new { pt.PostId, pt.TechStackId });

            modelBuilder.Entity<PostTechStack>()
                .HasRequired(pt => pt.Post)
                .WithMany(p => p.PostTechStacks)
                .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<PostTechStack>()
                .HasRequired(pt => pt.TechStack)
                .WithMany(t => t.PostTechStacks)
                .HasForeignKey(pt => pt.TechStackId);

            base.OnModelCreating(modelBuilder);
        }

    }
}