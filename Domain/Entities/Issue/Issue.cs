using Domain.Entities.Users;
using Shared.Enums;

namespace Domain.Entities.Issue
{
    public class Issue : BaseEntity<Guid>
    {
        public ProductType ProductType { get; set; }
        public string Model { get; set; } = string.Empty;
        public string? Processor { get; set; }
        public string? Ram { get; set; }
        public string? Storage { get; set; }
        public string? Gpu { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int? UserId { get; set; } 
        public Guid? AdminId { get; set; }
        public virtual User? User { get; set; }
        public virtual Admin.Admin? Admin { get; set; }
        public IssueStatus Status { get; set; } = IssueStatus.Pending;
        public string? AdminNotes { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
