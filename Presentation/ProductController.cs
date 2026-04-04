using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Dtos.Product;
using Shared.Enums;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ProductController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        #region Public GET Endpoints

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts(
            [FromQuery] bool trackChanges = false)
        {
            var result = await _serviceManager.ProductService.GetAllProductsAsync(trackChanges);
            return Ok(result);
        }

        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetActiveProducts()
        {
            var result = await _serviceManager.ProductService.GetActiveProductsAsync();
            return Ok(result);
        }

        [HttpGet("type/{productType}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByType(ProductType productType)
        {
            var result = await _serviceManager.ProductService.GetProductsByTypeAsync(productType);
            return Ok(result);
        }

        [HttpGet("condition/{condition}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCondition(DeviceCondition condition)
        {
            var result = await _serviceManager.ProductService.GetProductsByConditionAsync(condition);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
        {
            var result = await _serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("filter")]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResultDto<ProductDto>>> GetFilteredProducts(
            [FromBody] ProductFilterDto filterDto)
        {
            var result = await _serviceManager.ProductService.GetFilteredProductsAsync(filterDto);
            return Ok(result);
        }

        #endregion

        #region Price Calculation

        [HttpPost("calculate-price")]
        [AllowAnonymous]
        public async Task<ActionResult<PriceCalculationResultDto>> CalculatePrice(
            [FromBody] CalculatePriceDto calculateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _serviceManager.ProductService.CalculatePriceAsync(calculateDto);
            return Ok(result);
        }

        #endregion

        #region Checkers

        [HttpGet("exists/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> IsProductExists(Guid id)
        {
            var result = await _serviceManager.ProductService.IsProductExistsAsync(id);
            return Ok(result);
        }

        [HttpGet("check-name")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> IsProductNameExists(
            [FromQuery] string nameAr,
            [FromQuery] string nameEn)
        {
            var result = await _serviceManager.ProductService.IsProductNameExistsAsync(nameAr, nameEn);
            return Ok(result);
        }

        #endregion

        #region Admin Operations

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromForm] CreateProductDto createDto) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _serviceManager.ProductService.CreateProductAsync(createDto);
            return CreatedAtAction(nameof(GetProductById), new { id = result.Id }, result);
        }

        [HttpPut]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<ProductDto>> UpdateProduct([FromForm] UpdateProductDto updateDto) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _serviceManager.ProductService.UpdateProductAsync(updateDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            await _serviceManager.ProductService.DeleteProductAsync(id);
            return Ok(new { message = "Product deleted successfully", id });
        }

        [HttpDelete("{id}/soft")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult> SoftDeleteProduct(Guid id)
        {
            await _serviceManager.ProductService.SoftDeleteProductAsync(id);
            return Ok(new { message = "Product soft deleted successfully", id });
        }

        #endregion
    }
}