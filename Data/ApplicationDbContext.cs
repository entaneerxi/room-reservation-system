using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoomReservationSystem.Models;

namespace RoomReservationSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<MonthlyRental> MonthlyRentals { get; set; }
        public DbSet<UtilityBill> UtilityBills { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<PaymentChannel> PaymentChannels { get; set; }
        public DbSet<AdditionalOption> AdditionalOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision
            modelBuilder.Entity<Room>()
                .Property(r => r.DailyRate)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Room>()
                .Property(r => r.MonthlyRate)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Booking>()
                .Property(b => b.DiscountAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Promotion>()
                .Property(p => p.DiscountPercentage)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Promotion>()
                .Property(p => p.DiscountAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MonthlyRental>()
                .Property(m => m.MonthlyRent)
                .HasPrecision(18, 2);

            modelBuilder.Entity<UtilityBill>()
                .Property(u => u.WaterReading)
                .HasPrecision(18, 2);

            modelBuilder.Entity<UtilityBill>()
                .Property(u => u.ElectricityReading)
                .HasPrecision(18, 2);

            modelBuilder.Entity<UtilityBill>()
                .Property(u => u.WaterAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<UtilityBill>()
                .Property(u => u.ElectricityAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<UtilityBill>()
                .Property(u => u.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<AdditionalOption>()
                .Property(a => a.Price)
                .HasPrecision(18, 2);

            // Configure relationships
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MonthlyRental>()
                .HasOne(m => m.User)
                .WithMany(u => u.MonthlyRentals)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MonthlyRental>()
                .HasOne(m => m.Room)
                .WithMany(r => r.MonthlyRentals)
                .HasForeignKey(m => m.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
