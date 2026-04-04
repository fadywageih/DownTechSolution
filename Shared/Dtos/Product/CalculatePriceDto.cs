namespace Shared.Dtos.Product
{
    public class CalculatePriceDto
    {
        public Guid ProductId { get; set; }
        public List<SelectedUpgradeDto> SelectedUpgrades { get; set; } = new();
    }
}
