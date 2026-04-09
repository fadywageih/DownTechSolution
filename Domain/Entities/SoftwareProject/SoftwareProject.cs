using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Entities.SoftwareProject
{
    public class SoftwareProject : BaseEntity<Guid>
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public FrontendType FrontendType { get; set; }
        public string? FrontendLibrariesJson { get; set; } 
        public BackendType BackendType { get; set; }
        public BackendFramework? BackendFramework { get; set; }
        public DatabaseType? Database { get; set; }

        public string? GithubUrl { get; set; }
        public string? LiveDemoUrl { get; set; }
        public Guid? CreatedByAdminId { get; set; }
        public Guid? UpdatedByAdminId { get; set; }

        public virtual Admin.Admin? CreatedByAdmin { get; set; }
        public virtual Admin.Admin? UpdatedByAdmin { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public List<FrontendLibrary> GetFrontendLibraries()
        {
            if (string.IsNullOrEmpty(FrontendLibrariesJson))
                return new List<FrontendLibrary>();

            try
            {
                return JsonSerializer.Deserialize<List<FrontendLibrary>>(FrontendLibrariesJson) ?? new List<FrontendLibrary>();
            }
            catch
            {
                return new List<FrontendLibrary>();
            }
        }

        public void SetFrontendLibraries(List<FrontendLibrary> libraries)
        {
            FrontendLibrariesJson = JsonSerializer.Serialize(libraries);
        }
    }
}
