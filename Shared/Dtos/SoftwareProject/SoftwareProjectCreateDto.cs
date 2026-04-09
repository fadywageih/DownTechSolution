using Microsoft.AspNetCore.Http;
using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.SoftwareProject
{
    public class SoftwareProjectCreateDto
    {
        [Required(ErrorMessage = "اسم المشروع بالعربية مطلوب")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "الاسم يجب أن يكون بين 2 و 200 حرف")]
        public string NameAr { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم المشروع بالإنجليزية مطلوب")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "الاسم يجب أن يكون بين 2 و 200 حرف")]
        public string NameEn { get; set; } = string.Empty;

        [Required(ErrorMessage = "وصف المشروع بالعربية مطلوب")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "الوصف يجب أن يكون بين 10 و 2000 حرف")]
        public string DescriptionAr { get; set; } = string.Empty;

        [Required(ErrorMessage = "وصف المشروع بالإنجليزية مطلوب")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "الوصف يجب أن يكون بين 10 و 2000 حرف")]
        public string DescriptionEn { get; set; } = string.Empty;

        [Required(ErrorMessage = "صورة المشروع مطلوبة")]
        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "نوع الـ Frontend مطلوب")]
        public FrontendType FrontendType { get; set; }

        public List<FrontendLibrary>? FrontendLibraries { get; set; }

        [Required(ErrorMessage = "نوع الـ Backend مطلوب")]
        public BackendType BackendType { get; set; }

        public BackendFramework? BackendFramework { get; set; }

        public DatabaseType? Database { get; set; }

        [StringLength(500, ErrorMessage = "الحد الأقصى 500 حرف")]
        public string? GithubUrl { get; set; }

        [StringLength(500, ErrorMessage = "الحد الأقصى 500 حرف")]
        public string? LiveDemoUrl { get; set; }
    }
}
