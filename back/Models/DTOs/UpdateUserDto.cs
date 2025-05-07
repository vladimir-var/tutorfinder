using System.ComponentModel.DataAnnotations;

namespace tutorfinder.Models.DTOs
{
    public class UpdateUserDto
    {
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(6)]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }
} 