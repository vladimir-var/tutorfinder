using System.ComponentModel.DataAnnotations;

namespace tutorfinder.DTOs
{
    public class TutorDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Bio { get; set; }
        public string Education { get; set; }
        public int YearsOfExperience { get; set; }
        public decimal HourlyRate { get; set; }
        public bool IsAvailable { get; set; }
        public string? TeachingStyle { get; set; }
        public string? Certifications { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public UserDto User { get; set; }
        public ICollection<SubjectDto> Subjects { get; set; }
    }

    public class CreateTutorDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Bio { get; set; }

        [Required]
        public string Education { get; set; }

        [Required]
        [Range(0, 100)]
        public int YearsOfExperience { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal HourlyRate { get; set; }

        public string? TeachingStyle { get; set; }
        public string? Certifications { get; set; }
        public ICollection<int> SubjectIds { get; set; }
    }

    public class UpdateTutorDto
    {
        public string? Bio { get; set; }
        public string? Education { get; set; }
        public int? YearsOfExperience { get; set; }
        public decimal? HourlyRate { get; set; }
        public bool? IsAvailable { get; set; }
        public string? TeachingStyle { get; set; }
        public string? Certifications { get; set; }
        public ICollection<int>? SubjectIds { get; set; }
    }
} 