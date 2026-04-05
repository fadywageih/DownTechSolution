using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Issue
{
    public class IssueCreateDto
    {
        [Required(ErrorMessage = "نوع الجهاز مطلوب")]
        public ProductType ProductType { get; set; }

        [Required(ErrorMessage = "الموديل مطلوب")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "الموديل يجب أن يكون بين 2 و 100 حرف")]
        public string Model { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Processor { get; set; }

        [StringLength(50)]
        public string? Ram { get; set; }

        [StringLength(50)]
        public string? Storage { get; set; }

        [StringLength(100)]
        public string? Gpu { get; set; }

        [Required(ErrorMessage = "وصف المشكلة مطلوب")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "وصف المشكلة يجب أن يكون بين 10 و 2000 حرف")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "الاسم يجب أن يكون بين 2 و 100 حرف")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "رقم هاتف غير صحيح")]
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "رقم الهاتف يجب أن يكون رقم مصري صحيح")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
