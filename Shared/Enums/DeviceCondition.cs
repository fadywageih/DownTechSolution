using System.ComponentModel.DataAnnotations;

namespace Shared.Enums
{
    public enum DeviceCondition
    {
        [Display(Name = "New", Description = "جديد")]
        New = 1,

        [Display(Name = "Used", Description = "مستعمل")]
        Used = 2
    }
}
