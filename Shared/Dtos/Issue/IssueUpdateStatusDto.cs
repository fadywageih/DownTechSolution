using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Issue
{
    public class IssueUpdateStatusDto
    {
        [Required]
        public Guid IssueId { get; set; }

        [Required]
        public IssueStatus IssueStatus { get; set; } 

        [StringLength(1000)]
        public string? AdminNotes { get; set; }
    }
}
