using Shared.Dtos.Product;
using Shared.Enums;

namespace ServicesAbstraction
{
    public interface IProductService
    {
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<IReadOnlyList<ProductDto>> GetAllProductsAsync(bool trackChanges = false);
        Task<IReadOnlyList<ProductDto>> GetProductsByTypeAsync(ProductType productType);
        Task<IReadOnlyList<ProductDto>> GetProductsByConditionAsync(DeviceCondition condition);
        Task<IReadOnlyList<ProductDto>> GetActiveProductsAsync();
        Task<ProductDto> CreateProductAsync(CreateProductDto createDto);
        Task<ProductDto> UpdateProductAsync(UpdateProductDto updateDto);
        Task<bool> DeleteProductAsync(Guid id);
        Task<bool> SoftDeleteProductAsync(Guid id);

        Task<PriceCalculationResultDto> CalculatePriceAsync(CalculatePriceDto calculateDto);

        Task<PagedResultDto<ProductDto>> GetFilteredProductsAsync(ProductFilterDto filterDto);
        Task<bool> IsProductExistsAsync(Guid id);
        Task<bool> IsProductNameExistsAsync(string nameAr, string nameEn);
        Task CreateProductRequestAsync(CreateProductRequestDto dto);
        Task<IEnumerable<ProductRequestDto>> GetProductRequestsAsync();
    }
}
