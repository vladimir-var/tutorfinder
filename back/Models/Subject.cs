using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tutorfinder.Models
{
    public class Subject
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("category")]
        public string Category { get; set; }

        [Column("icon")]
        public string? Icon { get; set; }

        // Навигационные свойства
        public ICollection<TutorSubject> TutorSubjects { get; set; }
    }
} 