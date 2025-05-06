using System;
using System.ComponentModel.DataAnnotations;

namespace TutorFinder.Models
{
    public class Review
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
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(1000)]
        public string Comment { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsVerified { get; set; } = false;
    }
} 