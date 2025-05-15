using System.ComponentModel.DataAnnotations;

namespace tutorfinder.DTOs
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
    }

    public class CreateSubjectDto
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Icon { get; set; }
    }

    public class UpdateSubjectDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
    }
} 