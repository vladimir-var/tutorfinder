using System.ComponentModel.DataAnnotations;

namespace tutorfinder.Models.DTOs
{
    public class CreateTutorDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Education { get; set; }

        [Required]
        public string Experience { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal HourlyRate { get; set; }

        public bool IsAvailable { get; set; } = true;

        public List<int> SubjectIds { get; set; }
    }
} 