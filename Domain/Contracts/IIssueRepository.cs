using Domain.Entities.Issue;
using Shared.Dtos.Issue;
using Shared.Enums;

namespace Domain.Contracts
{
    public interface IIssueRepository : IGenericRepository<Issue, Guid>
    {
        Task<IEnumerable<Issue>> GetIssuesByUserIdAsync(int userId); 
        Task<IEnumerable<Issue>> GetIssuesByAdminIdAsync(Guid adminId);
        Task<IEnumerable<Issue>> GetIssuesWithDetailsAsync(IssueFilterDto? filter = null);
        Task<Issue?> GetIssueWithDetailsByIdAsync(Guid id);
        Task<int> GetTotalIssuesCountAsync(IssueFilterDto? filter = null);
        Task<Dictionary<ProductType, int>> GetIssuesCountByProductTypeAsync();
        Task<double> GetAverageResolutionTimeAsync();
    }
}
