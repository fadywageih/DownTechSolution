using Shared.Enums;

namespace Shared.Dtos.Product
{
    public class ProductFilterDto
    {
        public string? SearchTerm { get; set; }
        public ProductType? ProductType { get; set; }
        public DeviceCondition? Condition { get; set; }
        public AccessoryType? AccessoryType { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsActive { get; set; }
        public bool? AllowRamUpgrade { get; set; }
        public bool? AllowStorageUpgrade { get; set; }
        public bool? AllowGpuUpgrade { get; set; }

        // Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Sorting
        public string? SortBy { get; set; } // "price", "name", "createdAt"
        public bool SortDescending { get; set; } = false;
    }
}
