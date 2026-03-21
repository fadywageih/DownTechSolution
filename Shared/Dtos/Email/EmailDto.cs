using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Email
{
    public class EmailDto
    {
        [Required, EmailAddress]
        public string To { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
