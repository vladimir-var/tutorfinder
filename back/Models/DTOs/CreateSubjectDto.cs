using System.ComponentModel.DataAnnotations;

namespace tutorfinder.Models.DTOs
{
    public class CreateSubjectDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
} 