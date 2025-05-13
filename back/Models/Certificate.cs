using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tutorfinder.Models
{
    public class Certificate
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("tutorid")]
        public int TutorId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("organization")]
        public string Organization { get; set; }

        [Required]
        [Column("date")]
        public DateTime Date { get; set; }

        [Required]
        [Column("fileurl")]
        public string FileUrl { get; set; }

        // Навигационные свойства
        public Tutor Tutor { get; set; }
    }
} 