using System.ComponentModel.DataAnnotations;

namespace Shared.Enums
{

    public enum UpgradeType
    {
        [Display(Name = "RAM", Description = "رامات")]
        RAM = 1,

        [Display(Name = "Storage", Description = "تخزين")]
        Storage = 2,

        [Display(Name = "GPU", Description = "كارت شاشة")]
        GPU = 3
    }
}
