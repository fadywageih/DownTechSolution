using Shared.Enums;

namespace Shared.Dtos.SoftwareProject
{
    public class SoftwareProjectFilterDto
    {
        public string? SearchTerm { get; set; }
        public FrontendType? FrontendType { get; set; }
        public BackendType? BackendType { get; set; }
        public FrontendLibrary? FrontendLibrary { get; set; }
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 12;
    }
}
