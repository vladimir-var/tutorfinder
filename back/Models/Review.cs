using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tutorfinder.Models
{
    public class Review
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("tutorid")]
        public int TutorId { get; set; }

        [Required]
        [Column("studentid")]
        public int StudentId { get; set; }

        [Required]
        [Range(1, 5)]
        [Column("rating")]
        public int Rating { get; set; }

        [Column("comment")]
        public string? Comment { get; set; }

        [Required]
        [Column("isverified")]
        public bool IsVerified { get; set; } = false;

        // Навигационные свойства
        public Tutor Tutor { get; set; }
        public User Student { get; set; }
    }
} 