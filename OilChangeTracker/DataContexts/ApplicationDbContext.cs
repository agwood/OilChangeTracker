using Microsoft.AspNet.Identity.EntityFramework;
using OilChangeTracker.Models;
using System.Data.Entity;

namespace OilChangeTracker.DataContexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<OilChangeEvent> OilChanges { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Attendance>()
        //        .HasRequired(a => a.Gig)
        //        .WithMany()
        //        .WillCascadeOnDelete(false);

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}