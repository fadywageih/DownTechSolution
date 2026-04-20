using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Orders
{
    public class SoftwareProjectRequest : BaseEntity<Guid>
    {
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Guid SoftwareProjectId { get; set; }
        [MaxLength(500)]
        public string Details { get; set; } = string.Empty; 
        [Required]
        public string Status { get; set; } = "Pending"; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public SoftwareProject.SoftwareProject? SoftwareProject { get; set; }
    }
}
