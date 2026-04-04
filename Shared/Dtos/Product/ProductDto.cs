using Shared.Enums;

namespace Shared.Dtos.Product
{
    public class ProductDto : BaseDto<Guid>
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public ProductType ProductType { get; set; }
        public DeviceCondition Condition { get; set; }
        public bool IsActive { get; set; }
        public bool AllowRamUpgrade { get; set; }
        public bool AllowStorageUpgrade { get; set; }
        public bool AllowGpuUpgrade { get; set; }

        // For Accessories (optional)
        public AccessoryType? AccessoryType { get; set; }

        // Navigation Properties
        public List<ProductSpecificationDto> Specifications { get; set; } = new();
        public List<ProductMediaDto> Media { get; set; } = new();
        public List<ProductUpgradeDto> AvailableUpgrades { get; set; } = new();
    }
}
