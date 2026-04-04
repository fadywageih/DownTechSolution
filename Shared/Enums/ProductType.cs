using System.ComponentModel.DataAnnotations;

namespace Shared.Enums
{
    public enum ProductType
    {
        [Display(Name = "Laptop", Description = "لابتوب")]
        Laptop = 1,

        [Display(Name = "PC", Description = "كمبيوتر مكتبي")]
        PC = 2,

        [Display(Name = "Accessory", Description = "إكسسوار")]
        Accessory = 3
    }
}
