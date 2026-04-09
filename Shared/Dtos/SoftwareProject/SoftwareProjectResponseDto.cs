namespace Shared.Dtos.SoftwareProject
{
    public class SoftwareProjectResponseDto
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        public string FrontendType { get; set; } = string.Empty;
        public int FrontendTypeValue { get; set; }
        public List<string>? FrontendLibraries { get; set; }
        public string BackendType { get; set; } = string.Empty;
        public int BackendTypeValue { get; set; }
        public string? BackendFramework { get; set; }
        public int? BackendFrameworkValue { get; set; }
        public string? Database { get; set; }
        public int? DatabaseValue { get; set; }
        public string? GithubUrl { get; set; }
        public string? LiveDemoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
