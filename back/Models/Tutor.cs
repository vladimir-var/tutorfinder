using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tutorfinder.Models
{
    public class Tutor
    {
        public Tutor()
        {
            TutorSubjects = new List<TutorSubject>();
            Reviews = new List<Review>();
        }

        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("userid")]
        public int UserId { get; set; }

        [Required]
        [Column("bio")]
        public string Bio { get; set; }

        [Required]
        [Column("education")]
        public string Education { get; set; }

        [Required]
        [Column("yearsofexperience")]
        public int YearsOfExperience { get; set; }

        [Required]
        [Column("hourlyrate", TypeName = "decimal(10,2)")]
        [Range(0, 10000)]
        public decimal HourlyRate { get; set; }

        [Required]
        [Column("isavailable")]
        public bool IsAvailable { get; set; } = true;

        [Column("teachingstyle")]
        public string? TeachingStyle { get; set; }

        [Column("certifications")]
        public string? Certifications { get; set; }

        [Column("averagerating", TypeName = "decimal(3,2)")]
        public decimal AverageRating { get; set; } = 0;

        [Column("totalreviews")]
        public int TotalReviews { get; set; } = 0;

        // Навигационные свойства
        public User User { get; set; }
        public ICollection<TutorSubject> TutorSubjects { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
} 