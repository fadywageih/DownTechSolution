using Shared.Dtos.Issue;

namespace ServicesAbstraction
{
    public interface IIssueService
    {
Task<IssueResponseDto> CreateIssueAsync(IssueCreateDto createDto, int? userId = null);
        Task<IEnumerable<IssueResponseDto>> GetUserIssuesAsync(int userId); 
        Task<IssueResponseDto> GetIssueByIdAsync(Guid id);
        Task<IEnumerable<IssueResponseDto>> GetAllIssuesAsync(IssueFilterDto? filter = null);
        Task<IssueResponseDto> UpdateIssueStatusAsync(IssueUpdateStatusDto updateDto, Guid adminId);
        Task<IssueResponseDto> AssignIssueToAdminAsync(Guid issueId, Guid adminId);
        Task<IssueStatisticsDto> GetIssueStatisticsAsync();
        Task<bool> IssueExistsAsync(Guid id);
    }
}
