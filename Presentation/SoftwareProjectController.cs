using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Dtos.SoftwareProject;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class SoftwareProjectController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public SoftwareProjectController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        #region Public GET Endpoints (No Authentication Required)

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SoftwareProjectListDto>>> GetAllProjects(
            [FromQuery] SoftwareProjectFilterDto? filter)
        {
            var result = await _serviceManager.SoftwareProjectService.GetAllProjectsAsync(filter);
            return Ok(result);
        }

        [HttpGet("latest")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SoftwareProjectListDto>>> GetLatestProjects(
            [FromQuery] int count = 6)
        {
            var result = await _serviceManager.SoftwareProjectService.GetLatestProjectsAsync(count);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<SoftwareProjectResponseDto>> GetProjectById(Guid id)
        {
            var result = await _serviceManager.SoftwareProjectService.GetProjectByIdAsync(id);
            return Ok(result);
        }

        #endregion

        #region Admin Endpoints (Authentication Required)

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<SoftwareProjectResponseDto>> CreateProject([FromForm] SoftwareProjectCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var adminId = GetAdminIdFromClaims();

            if (createDto.Image == null || createDto.Image.Length == 0)
            {
                return BadRequest(new { message = "Project image is required" });
            }

            var result = await _serviceManager.SoftwareProjectService.CreateProjectAsync(createDto, adminId);
            return CreatedAtAction(nameof(GetProjectById), new { id = result.Id }, result);
        }

        [HttpPut]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<SoftwareProjectResponseDto>> UpdateProject([FromForm] SoftwareProjectUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var adminId = GetAdminIdFromClaims();
            var result = await _serviceManager.SoftwareProjectService.UpdateProjectAsync(updateDto, adminId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> DeleteProject(Guid id)
        {
            await _serviceManager.SoftwareProjectService.DeleteProjectAsync(id);
            return Ok(new { message = "Project deleted successfully", id });
        }

        [HttpDelete("{id}/soft")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> SoftDeleteProject(Guid id)
        {
            await _serviceManager.SoftwareProjectService.SoftDeleteProjectAsync(id);
            return Ok(new { message = "Project soft deleted successfully", id });
        }

        [HttpGet("statistics")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<SoftwareProjectStatisticsDto>> GetStatistics()
        {
            var result = await _serviceManager.SoftwareProjectService.GetStatisticsAsync();
            return Ok(result);
        }

        #endregion

        #region Software Project Requests

        [HttpPost("request")]
        [AllowAnonymous]
        public async Task<ActionResult> CreateSoftwareProjectRequest([FromBody] CreateSoftwareProjectRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _serviceManager.SoftwareProjectService.CreateSoftwareProjectRequestAsync(dto);
            return Ok(new { message = "Request created successfully" });
        }

        [HttpGet("admin/requests")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<IEnumerable<SoftwareProjectRequestDto>>> GetSoftwareProjectRequests()
        {
            var result = await _serviceManager.SoftwareProjectService.GetSoftwareProjectRequestsAsync();
            return Ok(result);
        }

        [HttpGet("admin/requests/{id}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<SoftwareProjectRequestDto>> GetRequestById(Guid id)
        {
            var result = await _serviceManager.SoftwareProjectService.GetSoftwareProjectRequestByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("admin/requests/{id}/status")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> UpdateRequestStatus(Guid id, [FromBody] UpdateSoftwareProjectRequestStatusDto dto)
        {
            if (id != dto.RequestId)
                return BadRequest(new { message = "ID mismatch" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _serviceManager.SoftwareProjectService.UpdateSoftwareProjectRequestStatusAsync(dto);
            return Ok(new { message = "Request status updated successfully" });
        }

        #endregion

        #region Private Helpers

        private Guid GetAdminIdFromClaims()
        {
            var adminIdClaim = User.FindFirst("admin_id")?.Value;
            if (string.IsNullOrEmpty(adminIdClaim) || !Guid.TryParse(adminIdClaim, out var adminId))
            {
                throw new UnauthorizedAccessException("Invalid admin ID in token");
            }
            return adminId;
        }

        #endregion
    }
}
