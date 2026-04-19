using Domain.Entities.Product;
using Shared.Enums;

namespace Domain.Contracts
{
    public interface IProductRepository : IGenericRepository<Product, Guid>
    {
        Task<Product?> GetProductWithDetailsAsync(Guid id);
        Task<IReadOnlyList<Product>> GetProductsByTypeAsync(ProductType productType);
        Task<IReadOnlyList<Product>> GetProductsByConditionAsync(DeviceCondition condition);
        Task<IReadOnlyList<Product>> GetProductsWithUpgradesAsync();
        Task<Product?> GetByNamesAsync(string nameAr, string nameEn);
        Task<IReadOnlyList<Product>> GetActiveProductsAsync();
Task<IReadOnlyList<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Domain.Entities.Orders.ProductRequest>> GetProductRequestsAsync();
    }
}
