using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Product
{
    public class ProductSpecification : BaseEntity<Guid>
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Key in Arabic is required")]
        [MaxLength(100, ErrorMessage = "Key cannot exceed 100 characters")]
        public string KeyAr { get; set; } = string.Empty;

        [Required(ErrorMessage = "Key in English is required")]
        [MaxLength(100, ErrorMessage = "Key cannot exceed 100 characters")]
        public string KeyEn { get; set; } = string.Empty;

        [Required(ErrorMessage = "Value in Arabic is required")]
        [MaxLength(500, ErrorMessage = "Value cannot exceed 500 characters")]
        public string ValueAr { get; set; } = string.Empty;

        [Required(ErrorMessage = "Value in English is required")]
        [MaxLength(500, ErrorMessage = "Value cannot exceed 500 characters")]
        public string ValueEn { get; set; } = string.Empty;
        public bool IsUpgradable { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
