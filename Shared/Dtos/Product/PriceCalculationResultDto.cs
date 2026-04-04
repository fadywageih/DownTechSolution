namespace Shared.Dtos.Product
{
    public class PriceCalculationResultDto
    {
        public Guid ProductId { get; set; }
        public string ProductNameAr { get; set; } = string.Empty;
        public string ProductNameEn { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public decimal UpgradesTotal { get; set; }
        public decimal FinalPrice { get; set; }
        public List<UpgradeBreakdownDto> UpgradeBreakdown { get; set; } = new();
    }
}
