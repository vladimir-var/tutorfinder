using Microsoft.EntityFrameworkCore;

namespace TutorFinder.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка связей для Tutor
            modelBuilder.Entity<Tutor>()
                .HasOne(t => t.User)
                .WithOne()
                .HasForeignKey<Tutor>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Настройка связей для Review
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Tutor)
                .WithMany(t => t.Reviews)
                .HasForeignKey(r => r.TutorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Student)
                .WithMany()
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка связей для Lesson
            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Tutor)
                .WithMany(t => t.Lessons)
                .HasForeignKey(l => l.TutorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Student)
                .WithMany()
                .HasForeignKey(l => l.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Subject)
                .WithMany(s => s.Lessons)
                .HasForeignKey(l => l.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка связей для Subject
            modelBuilder.Entity<Subject>()
                .HasMany(s => s.Tutors)
                .WithMany(t => t.Subjects)
                .UsingEntity(j => j.ToTable("TutorSubjects"));

            // Настройка индексов
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Tutor>()
                .HasIndex(t => t.UserId)
                .IsUnique();

            // Настройка значений по умолчанию
            modelBuilder.Entity<Tutor>()
                .Property(t => t.IsAvailable)
                .HasDefaultValue(true);

            modelBuilder.Entity<Review>()
                .Property(r => r.IsVerified)
                .HasDefaultValue(false);
        }
    }
} 