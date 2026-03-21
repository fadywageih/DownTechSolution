using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.User
{
    public class ForgetPasswordDto
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }
}
