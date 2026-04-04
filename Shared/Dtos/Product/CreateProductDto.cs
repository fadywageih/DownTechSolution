using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.Dtos.Product
{
    public class CreateProductDto
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public ProductType ProductType { get; set; }
        public DeviceCondition Condition { get; set; }
        public bool AllowRamUpgrade { get; set; }
        public bool AllowStorageUpgrade { get; set; }
        public bool AllowGpuUpgrade { get; set; }
        public List<IFormFile>? MediaFiles { get; set; }
        // For Accessories
        public AccessoryType? AccessoryType { get; set; }

        // Specifications
        public List<CreateProductSpecificationDto> Specifications { get; set; } = new();

        // Media
        public List<CreateProductMediaDto> Media { get; set; } = new();
    }
}
