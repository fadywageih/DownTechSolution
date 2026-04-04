using Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Product
{
    public class ProductUpgrade : BaseEntity<Guid>
    {
        [Required]
        public Guid ProductId { get; set; }
        public Guid? UpgradeOptionId { get; set; }

        [Required]
        public UpgradeType UpgradeType { get; set; }

        [Required(ErrorMessage = "From value is required")]
        [MaxLength(100, ErrorMessage = "From value cannot exceed 100 characters")]
        public string FromValue { get; set; } = string.Empty;

        [Required(ErrorMessage = "To value is required")]
        [MaxLength(100, ErrorMessage = "To value cannot exceed 100 characters")]
        public string ToValue { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Additional price must be greater than or equal to 0")]
        public decimal AdditionalPrice { get; set; }

        public bool IsActive { get; set; } = true;
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; } = null!;

        [ForeignKey(nameof(UpgradeOptionId))]
        public virtual UpgradeOption? UpgradeOption { get; set; }
    }
}
