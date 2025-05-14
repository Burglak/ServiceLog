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
        public DbSet<Token> Tokens { get; set; }

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

            modelBuilder.Entity<VehicleImage>()
                .HasOne(vi => vi.Vehicle)
                .WithMany()
                .HasForeignKey(vi => vi.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VehicleUser>()
                .HasKey(vu => new { vu.VehicleId, vu.UserId });

            modelBuilder.Entity<VehicleUser>()
                .HasOne(vu => vu.Vehicle)
                .WithMany()
                .HasForeignKey(vu => vu.VehicleId);

            modelBuilder.Entity<VehicleUser>()
                .HasOne(vu => vu.User)
                .WithMany()
                .HasForeignKey(vu => vu.UserId);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Vehicle)
                .WithMany()
                .HasForeignKey(n => n.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServiceRecord>()
                .HasOne(sr => sr.Vehicle)
                .WithMany()
                .HasForeignKey(sr => sr.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServiceRecordImage>()
                .HasOne(sri => sri.ServiceRecord)
                .WithMany()
                .HasForeignKey(sri => sri.ServiceRecordId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
