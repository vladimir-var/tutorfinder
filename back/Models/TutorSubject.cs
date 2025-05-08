using System.ComponentModel.DataAnnotations.Schema;

namespace tutorfinder.Models
{
    public class TutorSubject
    {
        [Column("tutorsid")]
        public int TutorsId { get; set; }

        [Column("subjectsid")]
        public int SubjectsId { get; set; }

        // Навигационные свойства
        public Tutor Tutor { get; set; }
        public Subject Subject { get; set; }
    }
} 