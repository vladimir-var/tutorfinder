using System.ComponentModel.DataAnnotations;

namespace tutorfinder.DTOs
{
    public class CertificateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        public DateTime Date { get; set; }
        public string FileUrl { get; set; }
    }

    public class CreateCertificateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Organization { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
} 