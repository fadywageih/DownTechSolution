namespace Shared.Dtos.SoftwareProject
{
    public class SoftwareProjectRequestDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public Guid SoftwareProjectId { get; set; }
        public string SoftwareProjectNameEn { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; }
    }
}
