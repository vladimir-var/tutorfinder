using System.ComponentModel.DataAnnotations;

namespace tutorfinder.Models.DTOs
{
    public class UpdateTutorDto
    {
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? HourlyRate { get; set; }

        public bool? IsAvailable { get; set; }
        public List<int> SubjectIds { get; set; }
    }
} 