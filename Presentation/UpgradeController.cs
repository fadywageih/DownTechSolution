using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Dtos.Product;
using Shared.Enums;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class UpgradeController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public UpgradeController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("product/{productId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductUpgradeDto>>> GetUpgradesForProduct(Guid productId)
        {
            var product = await _serviceManager.ProductService.GetProductByIdAsync(productId);
            return Ok(product.AvailableUpgrades);
        }

        [HttpPost("calculate")]
        [AllowAnonymous]
        public async Task<ActionResult<PriceCalculationResultDto>> CalculatePrice(
            [FromBody] CalculatePriceDto calculateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _serviceManager.ProductService.CalculatePriceAsync(calculateDto);
            return Ok(result);
        }

        [HttpGet("types")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<string>> GetUpgradeTypes()
        {
            var types = Enum.GetNames(typeof(UpgradeType));
            return Ok(types);
        }
    }
}
