using Shared.Enums;

namespace Domain.Entities.Product
{
    public class Accessory : Product
    {
        public AccessoryType AccessoryType { get; set; }
    }
}
