using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TutorFinder.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        public string Icon { get; set; }

        public virtual ICollection<Tutor> Tutors { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
} 