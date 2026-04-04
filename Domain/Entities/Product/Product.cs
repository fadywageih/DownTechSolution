using Shared.Enums;
namespace Domain.Entities.Product
{
    public class Product : BaseEntity<Guid>
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public ProductType ProductType { get; set; }
        public DeviceCondition Condition { get; set; }
        public bool IsActive { get; set; } = true;
        public bool AllowRamUpgrade { get; set; }
        public bool AllowStorageUpgrade { get; set; }
        public bool AllowGpuUpgrade { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<ProductSpecification> Specifications { get; set; } = new List<ProductSpecification>();
        public ICollection<ProductMedia> Media { get; set; } = new List<ProductMedia>();
        public ICollection<ProductUpgrade> ProductUpgrades { get; set; } = new List<ProductUpgrade>();
    }
}
