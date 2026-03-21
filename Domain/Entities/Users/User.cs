using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Users
{
    public class User : IdentityUser<int> 
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
