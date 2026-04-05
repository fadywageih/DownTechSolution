using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Dtos.Issue;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssueController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILogger<IssueController> _logger;

        public IssueController(IServiceManager serviceManager, ILogger<IssueController> logger)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }
        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult<IssueResponseDto>> CreateIssue([FromBody] IssueCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            // Log all claims for debugging
            var claims = User.Claims.ToList();
            _logger.LogInformation("Request claims count: {ClaimCount}", claims.Count);
            foreach (var claim in claims)
            {
                _logger.LogInformation("Claim - Type: {ClaimType}, Value: {ClaimValue}", claim.Type, claim.Value);
            }
            
            int? userId = ExtractUserIdFromClaims();
            _logger.LogInformation("Extracted UserId: {UserId}", userId?.ToString() ?? "NULL");
            
            if (!userId.HasValue)
            {
                _logger.LogWarning("Failed to extract user ID from claims");
                return Unauthorized(new { message = "Unable to identify user" });
            }

            var result = await _serviceManager.IssueService.CreateIssueAsync(createDto, userId);
            return Ok(result);
        }

        [HttpGet("user/{userId:int}")] 
        [Authorize]
        public async Task<ActionResult<IEnumerable<IssueResponseDto>>> GetUserIssues(int userId) 
        {
            var result = await _serviceManager.IssueService.GetUserIssuesAsync(userId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IssueResponseDto>> GetIssueById(Guid id)
        {
            var result = await _serviceManager.IssueService.GetIssueByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("admin/all")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<IEnumerable<IssueResponseDto>>> GetAllIssues([FromQuery] IssueFilterDto? filter)
        {
            var result = await _serviceManager.IssueService.GetAllIssuesAsync(filter);
            return Ok(result);
        }

        [HttpPut("admin/status")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<IssueResponseDto>> UpdateIssueStatus([FromBody] IssueUpdateStatusDto updateDto)
        {
            var adminId = GetAdminIdFromClaims();
            var result = await _serviceManager.IssueService.UpdateIssueStatusAsync(updateDto, adminId);
            return Ok(result);
        }

        [HttpPost("admin/assign/{issueId}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<IssueResponseDto>> AssignIssueToAdmin(Guid issueId)
        {
            var adminId = GetAdminIdFromClaims();
            var result = await _serviceManager.IssueService.AssignIssueToAdminAsync(issueId, adminId);
            return Ok(result);
        }

        [HttpGet("admin/statistics")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<IssueStatisticsDto>> GetStatistics()
        {
            var result = await _serviceManager.IssueService.GetIssueStatisticsAsync();
            return Ok(result);
        }

        private int? ExtractUserIdFromClaims()
        {
            // Try multiple claim type variations
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("userid")?.Value
                ?? User.FindFirst("nameid")?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;

            _logger.LogInformation("UserIdClaim found: {UserIdClaim}", userIdClaim ?? "NOT FOUND");

            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var uid))
            {
                _logger.LogInformation("Successfully parsed UserId: {UserId}", uid);
                return uid;
            }
            
            _logger.LogWarning("Failed to parse UserId from claim: {UserIdClaim}", userIdClaim ?? "NULL");
            return null;
        }

        private Guid GetAdminIdFromClaims()
        {
            var adminIdClaim = User.FindFirst("admin_id")?.Value;
            if (string.IsNullOrEmpty(adminIdClaim) || !Guid.TryParse(adminIdClaim, out var adminId))
            {
                throw new UnauthorizedAccessException("Invalid admin ID in token");
            }
            return adminId;
        }
    }
}
