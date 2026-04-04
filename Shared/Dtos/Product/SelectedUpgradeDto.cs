using Shared.Enums;

namespace Shared.Dtos.Product
{
    public class SelectedUpgradeDto
    {
        public Guid? UpgradeOptionId { get; set; }
        public UpgradeType UpgradeType { get; set; }
        public string ToValue { get; set; } = string.Empty;
    }
}
