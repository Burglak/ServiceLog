using Microsoft.EntityFrameworkCore;
using ServiceLog.Domain.Entities;

namespace ServiceLog.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ServiceRecord> ServiceRecords { get; set; }
        public DbSet<ServiceRecordImage> ServiceRecordImages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleImage> VehicleImages { get; set; }
        public DbSet<VehicleUser> VehicleUsers { get; set; }

        // automatically sets createdAt for newly added entities and updatedAt for modified entities
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
                }
                ((BaseEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // configuration for Vehicle and VehicleImage (One-to-Many)
            modelBuilder.Entity<VehicleImage>()
                .HasOne(vi => vi.Vehicle)
                .WithMany(v => v.VehicleImages)
                .HasForeignKey(vi => vi.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            // configuration for Vehicle and VehicleUser (Many-to-Many)
            modelBuilder.Entity<VehicleUser>()
                .HasKey(vu => new { vu.VehicleId, vu.UserId });

            modelBuilder.Entity<VehicleUser>()
                .HasOne(vu => vu.Vehicle)
                .WithMany(v => v.VehicleUsers)
                .HasForeignKey(vu => vu.VehicleId);

            modelBuilder.Entity<VehicleUser>()
                .HasOne(vu => vu.User)
                .WithMany(u => u.VehicleUsers)
                .HasForeignKey(vu => vu.UserId);

            // configuration for Vehicle and Notification (One-to-Many)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Vehicle)
                .WithMany(v => v.Notifications)
                .HasForeignKey(n => n.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete if Vehicle is deleted

            // configuration for Vehicle and ServiceRecord (One-to-Many)
            modelBuilder.Entity<ServiceRecord>()
                .HasOne(sr => sr.Vehicle)
                .WithMany(v => v.ServiceRecords)
                .HasForeignKey(sr => sr.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            // configuration for ServiceRecord and ServiceRecordImage (One-to-Many)
            modelBuilder.Entity<ServiceRecordImage>()
                .HasOne(sri => sri.ServiceRecord)
                .WithMany(sr => sr.serviceRecordImages)
                .HasForeignKey(sri => sri.ServiceRecordId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}