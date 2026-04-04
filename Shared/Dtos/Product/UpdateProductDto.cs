using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.Dtos.Product
{
    public class UpdateProductDto
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public DeviceCondition Condition { get; set; }
        public bool IsActive { get; set; }
        public bool AllowRamUpgrade { get; set; }
        public bool AllowStorageUpgrade { get; set; }
        public bool AllowGpuUpgrade { get; set; }
        public AccessoryType? AccessoryType { get; set; }
        public List<UpdateProductSpecificationDto> Specifications { get; set; } = new();
        public List<UpdateProductMediaDto> Media { get; set; } = new();
        public List<IFormFile>? MediaFiles { get; set; } // ✅ Add this
    }
}
