using Shared.Dtos.SoftwareProject;

namespace ServicesAbstraction
{
    public interface ISoftwareProjectService
    {
        Task<IEnumerable<SoftwareProjectListDto>> GetAllProjectsAsync(SoftwareProjectFilterDto? filter = null);
        Task<SoftwareProjectResponseDto> GetProjectByIdAsync(Guid id);
        Task<IEnumerable<SoftwareProjectListDto>> GetLatestProjectsAsync(int count);
        Task<SoftwareProjectResponseDto> CreateProjectAsync(SoftwareProjectCreateDto createDto, Guid adminId);
        Task<SoftwareProjectResponseDto> UpdateProjectAsync(SoftwareProjectUpdateDto updateDto, Guid adminId);
        Task<bool> DeleteProjectAsync(Guid id);
        Task<bool> SoftDeleteProjectAsync(Guid id);
        Task<SoftwareProjectStatisticsDto> GetStatisticsAsync();
        Task CreateSoftwareProjectRequestAsync(CreateSoftwareProjectRequestDto dto);
        Task<IEnumerable<SoftwareProjectRequestDto>> GetSoftwareProjectRequestsAsync();
        Task<SoftwareProjectRequestDto> GetSoftwareProjectRequestByIdAsync(Guid id);
        Task UpdateSoftwareProjectRequestStatusAsync(UpdateSoftwareProjectRequestStatusDto dto);
    }
}
