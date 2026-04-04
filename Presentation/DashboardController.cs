using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Dtos.Product;
using Shared.Enums;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public DashboardController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStatsDto>> GetDashboardStats()
        {
            var allProducts = await _serviceManager.ProductService.GetAllProductsAsync();
            var laptops = allProducts.Where(p => p.ProductType == ProductType.Laptop).ToList();
            var pcs = allProducts.Where(p => p.ProductType == ProductType.PC).ToList();
            var accessories = allProducts.Where(p => p.ProductType == ProductType.Accessory).ToList();

            var stats = new DashboardStatsDto
            {
                TotalProducts = allProducts.Count,
                TotalLaptops = laptops.Count,
                TotalPCs = pcs.Count,
                TotalAccessories = accessories.Count,
                NewProducts = allProducts.Count(p => p.Condition == DeviceCondition.New),
                UsedProducts = allProducts.Count(p => p.Condition == DeviceCondition.Used),
                ActiveProducts = allProducts.Count(p => p.IsActive),
                TotalRevenue = allProducts.Sum(p => p.BasePrice)
            };

            return Ok(stats);
        }

        [HttpGet("products-by-type")]
        public async Task<ActionResult<Dictionary<string, int>>> GetProductsByType()
        {
            var allProducts = await _serviceManager.ProductService.GetAllProductsAsync();

            var result = new Dictionary<string, int>
            {
                { "Laptops", allProducts.Count(p => p.ProductType == ProductType.Laptop) },
                { "PCs", allProducts.Count(p => p.ProductType == ProductType.PC) },
                { "Accessories", allProducts.Count(p => p.ProductType == ProductType.Accessory) }
            };

            return Ok(result);
        }

        [HttpGet("recent-products")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetRecentProducts(
            [FromQuery] int count = 10)
        {
            var filter = new ProductFilterDto
            {
                PageNumber = 1,
                PageSize = count,
                SortBy = "createdAt",
                SortDescending = true
            };

            var result = await _serviceManager.ProductService.GetFilteredProductsAsync(filter);
            return Ok(result.Items);
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetLowStockProducts()
        {
            // Note: You'll need to add stock tracking if needed
            var allProducts = await _serviceManager.ProductService.GetAllProductsAsync();
            var lowStock = allProducts.Take(10).ToList(); // Placeholder logic
            return Ok(lowStock);
        }
    }
}

public class DashboardStatsDto
{
    public int TotalProducts { get; set; }
    public int TotalLaptops { get; set; }
    public int TotalPCs { get; set; }
    public int TotalAccessories { get; set; }
    public int NewProducts { get; set; }
    public int UsedProducts { get; set; }
    public int ActiveProducts { get; set; }
    public decimal TotalRevenue { get; set; }
}
