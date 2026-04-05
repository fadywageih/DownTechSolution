using Shared.Enums;

namespace Shared.Dtos.Issue
{
    public class IssueFilterDto
    {
        public ProductType? ProductType { get; set; }
        public IssueStatus? Status { get; set; }  
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
