using Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Product
{
    public class UpgradeOption : BaseEntity<Guid>
    {
        [Required]
        public UpgradeType UpgradeType { get; set; }

        [Required(ErrorMessage = "Name in English is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string NameEn { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name in Arabic is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string NameAr { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        public ProductType? ApplicableTo { get; set; }
    }
}
