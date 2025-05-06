using System;
using System.ComponentModel.DataAnnotations;

namespace TutorFinder.Models
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TutorId { get; set; }
        public Tutor Tutor { get; set; }

        [Required]
        public int StudentId { get; set; }
        public User Student { get; set; }

        [Required]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } // Scheduled, Completed, Cancelled

        [Required]
        [Range(0, 1000)]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public string MeetingLink { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
} 