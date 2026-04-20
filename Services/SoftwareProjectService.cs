namespace Services
{
    public class SoftwareProjectService : ISoftwareProjectService
    {
        private readonly ISoftwareProjectRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExtendedImageService _imageService;
        private readonly IMapper _mapper;
        private readonly ILogger<SoftwareProjectService> _logger;

        public SoftwareProjectService(
            ISoftwareProjectRepository repository,
            IUnitOfWork unitOfWork,
            IExtendedImageService imageService,
            IMapper mapper,
            ILogger<SoftwareProjectService> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _mapper = mapper;
            _logger = logger;
        }

        #region User Endpoints

        public async Task<IEnumerable<SoftwareProjectListDto>> GetAllProjectsAsync(SoftwareProjectFilterDto? filter = null)
        {
            _logger.LogInformation("Getting all software projects with filter");
            var projects = await _repository.GetSoftwareProjectsWithFilterAsync(filter);
            return _mapper.Map<IEnumerable<SoftwareProjectListDto>>(projects);
        }

        public async Task<SoftwareProjectResponseDto> GetProjectByIdAsync(Guid id)
        {
            _logger.LogInformation("Getting software project by ID: {ProjectId}", id);
            var project = await _repository.GetSoftwareProjectWithDetailsAsync(id);

            if (project == null)
                throw new SoftwareProjectNotFoundException(id);

            return _mapper.Map<SoftwareProjectResponseDto>(project);
        }

        public async Task<IEnumerable<SoftwareProjectListDto>> GetLatestProjectsAsync(int count)
        {
            _logger.LogInformation("Getting latest {Count} software projects", count);
            var projects = await _repository.GetLatestProjectsAsync(count);
            return _mapper.Map<IEnumerable<SoftwareProjectListDto>>(projects);
        }

        #endregion

        #region Admin Endpoints

        public async Task<SoftwareProjectResponseDto> CreateProjectAsync(SoftwareProjectCreateDto createDto, Guid adminId)
        {
            _logger.LogInformation("Admin {AdminId} creating new software project: {NameEn}", adminId, createDto.NameEn);

            // Check if name already exists
            if (await _repository.IsNameExistsAsync(createDto.NameAr, createDto.NameEn))
                throw new Domain.Exceptions.ValidationException(new[] { $"Project with name '{createDto.NameEn}' already exists" });

            var project = _mapper.Map<SoftwareProject>(createDto);

            // Handle image upload
            if (createDto.Image != null && createDto.Image.Length > 0)
            {
                var imageUrl = await _imageService.SaveImageAsync(createDto.Image, "uploads/software");
                if (string.IsNullOrEmpty(imageUrl))
                    throw new Domain.Exceptions.ValidationException(new[] { "Failed to upload image" });
                project.ImageUrl = imageUrl;
            }
            else
            {
                throw new Domain.Exceptions.ValidationException(new[] { "Project image is required" });
            }

            // Handle FrontendLibraries
            if (createDto.FrontendLibraries != null && createDto.FrontendLibraries.Any())
            {
                project.SetFrontendLibraries(createDto.FrontendLibraries);
            }

            project.Id = Guid.NewGuid();
            project.CreatedByAdminId = adminId;
            project.CreatedAt = DateTime.UtcNow;

            await _repository.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Software project created successfully with ID: {ProjectId}", project.Id);
            return await GetProjectByIdAsync(project.Id);
        }

        public async Task<SoftwareProjectResponseDto> UpdateProjectAsync(SoftwareProjectUpdateDto updateDto, Guid adminId)
        {
            _logger.LogInformation("Admin {AdminId} updating software project: {ProjectId}", adminId, updateDto.Id);

            var project = await _repository.GetByIdAsync(updateDto.Id);
            if (project == null)
                throw new SoftwareProjectNotFoundException(updateDto.Id);

            // Check if name already exists (excluding current project)
            if (await _repository.IsNameExistsAsync(updateDto.NameAr, updateDto.NameEn, updateDto.Id))
                throw new Domain.Exceptions.ValidationException(new[] { $"Project with name '{updateDto.NameEn}' already exists" });

            // Handle image update
            if (updateDto.Image != null && updateDto.Image.Length > 0)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(project.ImageUrl))
                {
                    await _imageService.DeleteImageAsync(project.ImageUrl);
                }

                var imageUrl = await _imageService.SaveImageAsync(updateDto.Image, "uploads/software");
                if (string.IsNullOrEmpty(imageUrl))
                    throw new Domain.Exceptions.ValidationException(new[] { "Failed to upload image" });
                project.ImageUrl = imageUrl;
            }

            // Update properties
            project.NameAr = updateDto.NameAr;
            project.NameEn = updateDto.NameEn;
            project.DescriptionAr = updateDto.DescriptionAr;
            project.DescriptionEn = updateDto.DescriptionEn;
            project.FrontendType = updateDto.FrontendType;
            project.BackendType = updateDto.BackendType;
            project.BackendFramework = updateDto.BackendFramework;
            project.Database = updateDto.Database;
            project.GithubUrl = updateDto.GithubUrl;
            project.LiveDemoUrl = updateDto.LiveDemoUrl;
            project.UpdatedByAdminId = adminId;
            project.UpdatedAt = DateTime.UtcNow;

            // Handle FrontendLibraries
            if (updateDto.FrontendLibraries != null)
            {
                project.SetFrontendLibraries(updateDto.FrontendLibraries);
            }

            _repository.Update(project);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Software project updated successfully: {ProjectId}", project.Id);
            return await GetProjectByIdAsync(project.Id);
        }

        public async Task<bool> DeleteProjectAsync(Guid id)
        {
            _logger.LogInformation("Hard deleting software project: {ProjectId}", id);

            var project = await _repository.GetSoftwareProjectWithDetailsAsync(id);
            if (project == null)
                throw new SoftwareProjectNotFoundException(id);

            // Delete image from storage
            if (!string.IsNullOrEmpty(project.ImageUrl))
            {
                await _imageService.DeleteImageAsync(project.ImageUrl);
            }

            _repository.Delete(project);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Software project deleted successfully: {ProjectId}", id);
            return true;
        }

        public async Task<bool> SoftDeleteProjectAsync(Guid id)
        {
            _logger.LogInformation("Soft deleting software project: {ProjectId}", id);

            var project = await _repository.GetByIdAsync(id);
            if (project == null)
                throw new SoftwareProjectNotFoundException(id);

            project.IsDeleted = true;
            project.DeletedAt = DateTime.UtcNow;
            _repository.Update(project);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Software project soft deleted successfully: {ProjectId}", id);
            return true;
        }

        #endregion

        #region Software Project Requests

        public async Task CreateSoftwareProjectRequestAsync(CreateSoftwareProjectRequestDto dto)
        {
            _logger.LogInformation("Creating software project request for project {ProjectId}", dto.SoftwareProjectId);

            var project = await _repository.GetByIdAsync(dto.SoftwareProjectId);
            if (project == null)
                throw new SoftwareProjectNotFoundException(dto.SoftwareProjectId);

            var request = new Domain.Entities.Orders.SoftwareProjectRequest
            {
                Id = Guid.NewGuid(),
                UserName = dto.UserName,
                UserEmail = dto.UserEmail,
PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) ? null : dto.PhoneNumber.Trim(),
                SoftwareProjectId = dto.SoftwareProjectId,
                Details = dto.Details,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.GetRepository<Domain.Entities.Orders.SoftwareProjectRequest, Guid>().AddAsync(request);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Software project request created: {RequestId}", request.Id);
        }

        public async Task<IEnumerable<SoftwareProjectRequestDto>> GetSoftwareProjectRequestsAsync()
        {
            _logger.LogInformation("Getting all software project requests");

            var requests = await _repository.GetSoftwareProjectRequestsAsync();

            return _mapper.Map<IEnumerable<SoftwareProjectRequestDto>>(requests);
        }

        public async Task<SoftwareProjectRequestDto> GetSoftwareProjectRequestByIdAsync(Guid id)
        {
            _logger.LogInformation("Getting software project request by ID: {RequestId}", id);

            // استخدام GetByIdAsync وبعدين نجيب الـ SoftwareProject يدوي
            var request = await _unitOfWork.GetRepository<Domain.Entities.Orders.SoftwareProjectRequest, Guid>()
                .GetByIdAsync(id);

            if (request == null)
                throw new RequestNotFoundException(id);

            // نجيب تفاصيل المشروع المرتبط
            if (request.SoftwareProjectId != Guid.Empty)
            {
                request.SoftwareProject = await _repository.GetByIdAsync(request.SoftwareProjectId);
            }

            return _mapper.Map<SoftwareProjectRequestDto>(request);
        }

        public async Task UpdateSoftwareProjectRequestStatusAsync(UpdateSoftwareProjectRequestStatusDto dto)
        {
            _logger.LogInformation("Updating request {RequestId} status to {Status}", dto.RequestId, dto.Status);

            var request = await _unitOfWork.GetRepository<Domain.Entities.Orders.SoftwareProjectRequest, Guid>()
                .GetByIdAsync(dto.RequestId);

            if (request == null)
                throw new RequestNotFoundException(dto.RequestId);

            request.Status = dto.Status;
            _unitOfWork.GetRepository<Domain.Entities.Orders.SoftwareProjectRequest, Guid>().Update(request);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Request status updated successfully: {RequestId}", dto.RequestId);
        }

        #endregion

        #region Statistics

        public async Task<SoftwareProjectStatisticsDto> GetStatisticsAsync()
        {
            _logger.LogInformation("Getting software projects statistics");

            var totalProjects = await _repository.GetTotalCountAsync();
            var projectsByFrontend = await _repository.GetProjectsCountByFrontendTypeAsync();
            var projectsByBackend = await _repository.GetProjectsCountByBackendTypeAsync();
            var latestProjects = await _repository.GetLatestProjectsAsync(1);

            var frontendStats = projectsByFrontend.ToDictionary(
                k => GetFrontendTypeName(k.Key),
                v => v.Value);

            var backendStats = projectsByBackend.ToDictionary(
                k => GetBackendTypeName(k.Key),
                v => v.Value);

            return new SoftwareProjectStatisticsDto
            {
                TotalProjects = totalProjects,
                ProjectsByFrontendType = frontendStats,
                ProjectsByBackendType = backendStats,
                TotalAngularProjects = projectsByFrontend.GetValueOrDefault(FrontendType.Angular),
                TotalReactProjects = projectsByFrontend.GetValueOrDefault(FrontendType.React),
                TotalDotNetProjects = projectsByBackend.GetValueOrDefault(BackendType.DotNet),
                TotalNodeProjects = projectsByBackend.GetValueOrDefault(BackendType.NodeJS),
                TotalPythonProjects = projectsByBackend.GetValueOrDefault(BackendType.Python),
                LatestProjectDate = latestProjects.FirstOrDefault()?.CreatedAt ?? DateTime.UtcNow
            };
        }

        #endregion

        #region Helpers

        private static string GetFrontendTypeName(FrontendType type)
        {
            return type switch
            {
                FrontendType.Angular => "Angular",
                FrontendType.React => "React",
                FrontendType.Vue => "Vue.js",
                FrontendType.VanillaJS => "Vanilla JS",
                FrontendType.Other => "Other",
                _ => type.ToString()
            };
        }

        private static string GetBackendTypeName(BackendType type)
        {
            return type switch
            {
                BackendType.DotNet => ".NET",
                BackendType.NodeJS => "Node.js",
                BackendType.Python => "Python",
                BackendType.PHP => "PHP",
                BackendType.Other => "Other",
                _ => type.ToString()
            };
        }

        #endregion
    }
}