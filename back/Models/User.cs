using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tutorfinder.Models
{
    public class User
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("passwordhash")]
        public string PasswordHash { get; set; }

        [Required]
        [Column("firstname")]
        public string FirstName { get; set; }

        [Required]
        [Column("lastname")]
        public string LastName { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("profileimage")]
        public string? ProfileImage { get; set; }

        [Required]
        [Column("role")]
        public string Role { get; set; }

        // Навигационные свойства
        public Tutor Tutor { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
} 