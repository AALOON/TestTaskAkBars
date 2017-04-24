using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using MobileApi.Server.Core.Domain;

namespace MobileApi.Server.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<UserProfile> UserProfiles { get; set; }

        public ApplicationDbContext()
            : base("ApplicationDbContext", throwIfV1Schema: false)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ApplicationUser & UserProfile entity
            modelBuilder.Entity<ApplicationUser>()
                        .HasOptional(s => s.Profile) // Profile property optional in ApplicationUser entity
                        .WithRequired(ad => ad.User); // User property as required in UserProfile entity.

        }
    }
}
