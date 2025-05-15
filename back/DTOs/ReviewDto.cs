using System.ComponentModel.DataAnnotations;

namespace tutorfinder.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int TutorId { get; set; }
        public int StudentId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public bool IsVerified { get; set; }
        public UserDto Student { get; set; }
        public TutorDto Tutor { get; set; }
    }

    public class CreateReviewDto
    {
        [Required]
        public int TutorId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }
    }

    public class UpdateReviewDto
    {
        [Range(1, 5)]
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public bool? IsVerified { get; set; }
    }
} 