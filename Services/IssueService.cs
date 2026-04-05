namespace Services
{
    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<IssueService> _logger;

        public IssueService(
            IIssueRepository issueRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<IssueService> logger)
        {
            _issueRepository = issueRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IssueResponseDto> CreateIssueAsync(IssueCreateDto createDto, int? userId = null)
        {
            _logger.LogInformation("Creating new issue for customer: {CustomerName}, UserId: {UserId}", 
                createDto.CustomerName, userId?.ToString() ?? "NULL");

            var issue = _mapper.Map<Issue>(createDto);
            if (userId.HasValue)
            {
                issue.UserId = userId.Value;
                _logger.LogInformation("UserId assigned to issue: {UserId}", userId.Value);
            }
            else
            {
                _logger.LogWarning("No UserId provided - issue will be created with NULL UserId");
            }
            
            issue.Id = Guid.NewGuid();
            issue.CreatedAt = DateTime.UtcNow;
            issue.Status = IssueStatus.Pending;

            await _issueRepository.AddAsync(issue);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Issue created successfully with ID: {IssueId}, UserId: {UserId}", 
                issue.Id, issue.UserId?.ToString() ?? "NULL");
            return await GetIssueByIdAsync(issue.Id);
        }
        public async Task<IEnumerable<IssueResponseDto>> GetUserIssuesAsync(int userId) 
        {
            _logger.LogInformation("Getting all issues for user: {UserId}", userId);
            var issues = await _issueRepository.GetIssuesByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<IssueResponseDto>>(issues);
        }

        public async Task<IssueResponseDto> GetIssueByIdAsync(Guid id)
        {
            var issue = await _issueRepository.GetIssueWithDetailsByIdAsync(id);
            if (issue == null)
                throw new IssueNotFoundException(id);

            return _mapper.Map<IssueResponseDto>(issue);
        }

        public async Task<IEnumerable<IssueResponseDto>> GetAllIssuesAsync(IssueFilterDto? filter = null)
        {
            _logger.LogInformation("Getting all issues with filter");
            var issues = await _issueRepository.GetIssuesWithDetailsAsync(filter);
            return _mapper.Map<IEnumerable<IssueResponseDto>>(issues);
        }

        public async Task<IssueResponseDto> UpdateIssueStatusAsync(IssueUpdateStatusDto updateDto, Guid adminId)
        {
            // ✅ استخدم IssueStatus بدلاً من Status
            _logger.LogInformation("Updating issue status: {IssueId} to {IssueStatus}", updateDto.IssueId, updateDto.IssueStatus);

            var issue = await _issueRepository.GetByIdAsync(updateDto.IssueId);
            if (issue == null)
                throw new IssueNotFoundException(updateDto.IssueId);

            // ✅ استخدم IssueStatus
            issue.Status = updateDto.IssueStatus;
            issue.AdminNotes = updateDto.AdminNotes;
            issue.AdminId = adminId;
            issue.UpdatedAt = DateTime.UtcNow;

            // ✅ استخدم IssueStatus
            if (updateDto.IssueStatus == IssueStatus.Resolved)
                issue.ResolvedAt = DateTime.UtcNow;

            _issueRepository.Update(issue);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Issue status updated successfully");
            return await GetIssueByIdAsync(issue.Id);
        }

        public async Task<IssueResponseDto> AssignIssueToAdminAsync(Guid issueId, Guid adminId)
        {
            _logger.LogInformation("Assigning issue {IssueId} to admin {AdminId}", issueId, adminId);

            var issue = await _issueRepository.GetByIdAsync(issueId);
            if (issue == null)
                throw new IssueNotFoundException(issueId);

            issue.AdminId = adminId;
            issue.AssignedAt = DateTime.UtcNow;
            issue.Status = IssueStatus.InProgress;
            issue.UpdatedAt = DateTime.UtcNow;

            _issueRepository.Update(issue);
            await _unitOfWork.SaveChangesAsync();

            return await GetIssueByIdAsync(issueId);
        }

        public async Task<IssueStatisticsDto> GetIssueStatisticsAsync()
        {
            _logger.LogInformation("Getting issue statistics");

            var totalCount = await _issueRepository.GetTotalIssuesCountAsync();
            var issuesByType = await _issueRepository.GetIssuesCountByProductTypeAsync();
            var avgResolutionTime = await _issueRepository.GetAverageResolutionTimeAsync();

            var pendingCount = await _issueRepository.GetTotalIssuesCountAsync(new IssueFilterDto { Status = IssueStatus.Pending });
            var inProgressCount = await _issueRepository.GetTotalIssuesCountAsync(new IssueFilterDto { Status = IssueStatus.InProgress });
            var resolvedCount = await _issueRepository.GetTotalIssuesCountAsync(new IssueFilterDto { Status = IssueStatus.Resolved });
            var closedCount = await _issueRepository.GetTotalIssuesCountAsync(new IssueFilterDto { Status = IssueStatus.Closed });

            return new IssueStatisticsDto
            {
                TotalIssues = totalCount,
                PendingCount = pendingCount,
                InProgressCount = inProgressCount,
                ResolvedCount = resolvedCount,
                ClosedCount = closedCount,
                IssuesByProductType = issuesByType.ToDictionary(k => k.Key.ToString(), v => v.Value),
                AverageResolutionTimeHours = avgResolutionTime
            };
        }

        public async Task<bool> IssueExistsAsync(Guid id)
        {
            return await _issueRepository.GetByIdAsync(id) != null;
        }
    }
}
