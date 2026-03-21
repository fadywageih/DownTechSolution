using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.User
{
    public class RegisterDto
    {
        [Required] public string? Name { get; set; }
        [Required, EmailAddress] public string? Email { get; set; }
        [Required, DataType(DataType.Password)] public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare("Password")] public string ConfirmPassword { get; set; }
        public string? Phone { get; set; }  // Optional field

    }
}
