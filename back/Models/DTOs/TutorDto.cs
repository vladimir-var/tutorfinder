using System;
using System.Collections.Generic;

namespace tutorfinder.Models.DTOs
{
    public class TutorDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Description { get; set; }
        public decimal HourlyRate { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<SubjectDto> Subjects { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }
} 