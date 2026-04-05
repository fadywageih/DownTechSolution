namespace Shared.Dtos.Issue
{
    public class IssueStatisticsDto
    {
        public int TotalIssues { get; set; }
        public int PendingCount { get; set; }
        public int InProgressCount { get; set; }
        public int ResolvedCount { get; set; }
        public int ClosedCount { get; set; }
        public Dictionary<string, int> IssuesByProductType { get; set; } = new();
        public double AverageResolutionTimeHours { get; set; }
    }
}
