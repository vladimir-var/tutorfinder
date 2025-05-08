using Microsoft.EntityFrameworkCore;

namespace tutorfinder.Models
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
        public DbSet<TutorSubject> TutorSubjects { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Установка названий таблиц с маленькой буквы
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Tutor>().ToTable("tutors");
            modelBuilder.Entity<Subject>().ToTable("subjects");
            modelBuilder.Entity<TutorSubject>().ToTable("tutor_subjects");
            modelBuilder.Entity<Review>().ToTable("reviews");

            // Настройка связи многие-ко-многим для Tutor и Subject
            modelBuilder.Entity<TutorSubject>()
                .HasKey(ts => new { ts.TutorsId, ts.SubjectsId });

            modelBuilder.Entity<TutorSubject>()
                .HasOne(ts => ts.Tutor)
                .WithMany(t => t.TutorSubjects)
                .HasForeignKey(ts => ts.TutorsId);

            modelBuilder.Entity<TutorSubject>()
                .HasOne(ts => ts.Subject)
                .WithMany(s => s.TutorSubjects)
                .HasForeignKey(ts => ts.SubjectsId);

            // Настройка связи один-к-одному для User и Tutor
            modelBuilder.Entity<Tutor>()
                .HasOne(t => t.User)
                .WithOne(u => u.Tutor)
                .HasForeignKey<Tutor>(t => t.UserId);

            // Настройка связи для Reviews
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Tutor)
                .WithMany(t => t.Reviews)
                .HasForeignKey(r => r.TutorId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Student)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.StudentId);
        }
    }
} 