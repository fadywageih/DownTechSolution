using Shared.Enums;

namespace Shared.Dtos.Product
{
    public class UpgradeBreakdownDto
    {
        public UpgradeType UpgradeType { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string FromValue { get; set; } = string.Empty;
        public string ToValue { get; set; } = string.Empty;
        public decimal AdditionalPrice { get; set; }
    }
}
