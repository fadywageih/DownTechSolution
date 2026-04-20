using System.ComponentModel.DataAnnotations;
namespace Shared.Dtos.SoftwareProject
{
    public class UpdateSoftwareProjectRequestStatusDto
    {
        [Required]
        public Guid RequestId { get; set; }

        [Required]
        [RegularExpression("^(Contacted|Delivered|Rejected)$", ErrorMessage = "Status must be either Contacted, Delivered, or Rejected")]
        public string Status { get; set; } = string.Empty;
    }
}
