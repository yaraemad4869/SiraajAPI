using Microsoft.EntityFrameworkCore;
using SiraajApi.Models;

namespace SiraajApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Guardian> Guardians { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<QuranProgram> Programs { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<HeroSettings> HeroSettings { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<FamilyDiscount> FamilyDiscounts { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<StudentProgram> StudentPrograms { get; set; }
        public DbSet<ReferralDiscount> ReferralDiscounts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Phone)
                .IsUnique();
            
            modelBuilder.Entity<User>()
                .HasIndex(u => u.ReferralCode)
                .IsUnique();

            // Student configuration
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.UserId);

                entity.HasOne(s => s.User)
                    .WithOne(u => u.Student)
                    .HasForeignKey<Student>(s => s.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.Teacher)
                    .WithMany(u => u.Students)
                    .HasForeignKey(s => s.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Guardian)
                    .WithMany(u => u.Children)
                    .HasForeignKey(s => s.GuardianId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Guardian configuration
            modelBuilder.Entity<Guardian>(entity =>
            {
                entity.HasKey(g => g.UserId);

                entity.HasOne(g => g.User)
                    .WithOne(u => u.Guardian)
                    .HasForeignKey<Guardian>(g => g.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Teacher configuration
            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(t => t.UserId);

                entity.HasOne(t => t.User)
                    .WithOne(u => u.Teacher)
                    .HasForeignKey<Teacher>(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // RefreshToken configuration
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasIndex(rt => rt.Token)
                    .IsUnique();

                entity.HasOne(rt => rt.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // DiscountCode configuration
            modelBuilder.Entity<DiscountCode>(entity =>
            {
                entity.HasIndex(dc => dc.Code)
                    .IsUnique();

                entity.HasOne(dc => dc.User)
                    .WithMany()
                    .HasForeignKey(dc => dc.UsedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Schedule configuration
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasOne(s => s.Student)
                    .WithMany()
                    .HasForeignKey(s => s.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // StudentProgram configuration
            modelBuilder.Entity<StudentProgram>(entity =>
            {
                entity.HasOne(sp => sp.Student)
                    .WithMany()
                    .HasForeignKey(sp => sp.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sp => sp.Program)
                    .WithMany()
                    .HasForeignKey(sp => sp.ProgramId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sp => sp.Package)
                    .WithMany()
                    .HasForeignKey(sp => sp.PackageId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ReferralDiscount configuration
            modelBuilder.Entity<ReferralDiscount>(entity =>
            {
                entity.HasOne(rd => rd.Referrer)
                    .WithMany()
                    .HasForeignKey(rd => rd.ReferrerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(rd => rd.ReferredUser)
                    .WithMany()
                    .HasForeignKey(rd => rd.ReferredUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Payment configuration
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasOne(p => p.Student)
                    .WithMany()
                    .HasForeignKey(p => p.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.StudentProgram)
                    .WithMany()
                    .HasForeignKey(p => p.StudentProgramId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Invoice configuration
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasOne(i => i.Payment)
                    .WithMany()
                    .HasForeignKey(i => i.PaymentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(i => i.Student)
                    .WithMany()
                    .HasForeignKey(i => i.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Subscription configuration
            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasOne(s => s.Student)
                    .WithMany()
                    .HasForeignKey(s => s.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Package)
                    .WithMany()
                    .HasForeignKey(s => s.PackageId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Program)
                    .WithMany()
                    .HasForeignKey(s => s.ProgramId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Package configuration
            modelBuilder.Entity<Package>()
                .ToTable("Packages");

            // FamilyDiscount configuration
            modelBuilder.Entity<FamilyDiscount>()
                .ToTable("FamilyDiscounts");
        }
    }
}
