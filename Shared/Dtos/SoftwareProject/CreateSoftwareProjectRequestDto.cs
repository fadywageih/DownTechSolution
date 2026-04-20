namespace Shared.Dtos.SoftwareProject
{
    public class CreateSoftwareProjectRequestDto
    {
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Guid SoftwareProjectId { get; set; }
        public string Details { get; set; } = string.Empty;
    }
}
