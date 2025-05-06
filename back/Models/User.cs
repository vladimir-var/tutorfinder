using System;
using System.ComponentModel.DataAnnotations;

namespace TutorFinder.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string PasswordHash { get; set; }

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public string ProfileImage { get; set; }
    }
} 