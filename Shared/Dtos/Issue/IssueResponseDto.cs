using Shared.Enums;

namespace Shared.Dtos.Issue
{
    public class IssueResponseDto
    {
        public Guid Id { get; set; }
        public ProductType ProductType { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? Processor { get; set; }
        public string? Ram { get; set; }
        public string? Storage { get; set; }
        public string? Gpu { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; 
        public int StatusValue { get; set; } 
        public string? AdminNotes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string? UserEmail { get; set; }
        public string? AssignedAdminName { get; set; }
    }
}
