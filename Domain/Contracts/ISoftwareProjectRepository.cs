using Domain.Entities.SoftwareProject;
using Shared.Dtos.SoftwareProject;
using Shared.Enums;

namespace Domain.Contracts
{
    public interface ISoftwareProjectRepository : IGenericRepository<SoftwareProject, Guid>
    {
        Task<SoftwareProject?> GetSoftwareProjectWithDetailsAsync(Guid id);
        Task<IEnumerable<SoftwareProject>> GetSoftwareProjectsWithFilterAsync(SoftwareProjectFilterDto? filter = null);
        Task<int> GetTotalCountAsync(SoftwareProjectFilterDto? filter = null);
        Task<IEnumerable<SoftwareProject>> GetLatestProjectsAsync(int count);
        Task<Dictionary<FrontendType, int>> GetProjectsCountByFrontendTypeAsync();
        Task<Dictionary<BackendType, int>> GetProjectsCountByBackendTypeAsync();
        Task<bool> IsNameExistsAsync(string nameAr, string nameEn, Guid? excludeId = null);
    }
}
