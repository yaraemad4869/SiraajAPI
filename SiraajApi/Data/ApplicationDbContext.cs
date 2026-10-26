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
        
        // NEW TABLES
        public DbSet<Supervisor> Supervisors { get; set; }
        public DbSet<StudentDetails> StudentDetails { get; set; }
        public DbSet<SessionFeedback> SessionFeedbacks { get; set; }
        public DbSet<SurahCompletionReport> SurahCompletionReports { get; set; }
        public DbSet<QuranSurah> QuranSurahs { get; set; }
        public DbSet<TajweedRule> TajweedRules { get; set; }

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

            // Supervisor configuration
            modelBuilder.Entity<Supervisor>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.HasOne(s => s.User)
                    .WithMany()
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(s => s.Email)
                    .IsUnique();
            });

            // StudentDetails configuration
            modelBuilder.Entity<StudentDetails>(entity =>
            {
                entity.HasKey(sd => sd.Id);

                entity.HasOne(sd => sd.Student)
                    .WithMany()
                    .HasForeignKey(sd => sd.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sd => sd.Supervisor)
                    .WithMany(s => s.Students)
                    .HasForeignKey(sd => sd.SupervisorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sd => sd.Teacher)
                    .WithMany()
                    .HasForeignKey(sd => sd.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // SessionFeedback configuration
            modelBuilder.Entity<SessionFeedback>(entity =>
            {
                entity.HasKey(sf => sf.Id);

                entity.HasOne(sf => sf.Student)
                    .WithMany()
                    .HasForeignKey(sf => sf.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sf => sf.Teacher)
                    .WithMany()
                    .HasForeignKey(sf => sf.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // SurahCompletionReport configuration
            modelBuilder.Entity<SurahCompletionReport>(entity =>
            {
                entity.HasKey(sc => sc.Id);

                entity.HasOne(sc => sc.Student)
                    .WithMany()
                    .HasForeignKey(sc => sc.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sc => sc.Teacher)
                    .WithMany()
                    .HasForeignKey(sc => sc.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // QuranSurah configuration
            modelBuilder.Entity<QuranSurah>(entity =>
            {
                entity.HasIndex(qs => qs.Number)
                    .IsUnique();
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
