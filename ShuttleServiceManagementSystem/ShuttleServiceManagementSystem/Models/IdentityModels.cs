using Microsoft.AspNet.Identity.EntityFramework;
using SSMSDataModel.DAL;

namespace ShuttleServiceManagementSystem.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("4MoxieDatabaseConnection")
        {

        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("USER_ACCOUNTS", "4Moxie").Property(p => p.Id).HasColumnName("USER_ID");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER_ACCOUNTS", "4Moxie").Property(p => p.Id).HasColumnName("USER_ID");
            modelBuilder.Entity<IdentityUserRole>().ToTable("USER_ROLES", "4Moxie");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("USER_LOGINS", "4Moxie");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("USER_CLAIMS", "4Moxie");
            modelBuilder.Entity<IdentityRole>().ToTable("SYSTEM_ROLES", "4Moxie");
        }
    }
}