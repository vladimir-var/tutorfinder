using System.ComponentModel.DataAnnotations;

namespace tutorfinder.Models.DTOs
{
    public class CreateReviewDto
    {
        [Required]
        public int TutorId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; }
    }
} 