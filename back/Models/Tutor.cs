using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TutorFinder.Models
{
    public class Tutor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [StringLength(500)]
        public string Bio { get; set; }

        [Required]
        [StringLength(200)]
        public string Education { get; set; }

        [Required]
        public int YearsOfExperience { get; set; }

        [Required]
        [Range(0, 1000)]
        public decimal HourlyRate { get; set; }

        public bool IsAvailable { get; set; } = true;

        public string TeachingStyle { get; set; }

        public string Certifications { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }

        public double AverageRating { get; set; }

        public int TotalReviews { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
} 