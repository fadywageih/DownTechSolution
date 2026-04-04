namespace Shared.Dtos.Product
{
    public class ProductSpecificationDto
    {
        public string KeyAr { get; set; } = string.Empty;
        public string KeyEn { get; set; } = string.Empty;
        public string ValueAr { get; set; } = string.Empty;
        public string ValueEn { get; set; } = string.Empty;
        public bool IsUpgradable { get; set; }
    }
}
