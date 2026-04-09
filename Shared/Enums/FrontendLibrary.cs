using System.ComponentModel.DataAnnotations;

namespace Shared.Enums
{
    public enum FrontendLibrary
    {
        [Display(Name = "Tailwind CSS", Description = "Tailwind CSS")]
        Tailwind = 1,

        [Display(Name = "Bootstrap", Description = "Bootstrap")]
        Bootstrap = 2,

        [Display(Name = "Material UI", Description = "Material UI")]
        MaterialUI = 3,

        [Display(Name = "Ant Design", Description = "Ant Design")]
        AntDesign = 4,

        [Display(Name = "Chakra UI", Description = "Chakra UI")]
        ChakraUI = 5,

        [Display(Name = "None", Description = "بدون مكتبات")]
        None = 6,

        [Display(Name = "Other", Description = "أخرى")]
        Other = 7
    }
}
