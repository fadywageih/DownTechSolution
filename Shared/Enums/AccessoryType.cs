using System.ComponentModel.DataAnnotations;

namespace Shared.Enums
{
    public enum AccessoryType
    {
        [Display(Name = "Monitor", Description = "شاشة")]
        Monitor = 1,

        [Display(Name = "Keyboard", Description = "كيبورد")]
        Keyboard = 2,

        [Display(Name = "Mouse", Description = "ماوس")]
        Mouse = 3,

        [Display(Name = "Printer", Description = "طابعة")]
        Printer = 4
    }
}
