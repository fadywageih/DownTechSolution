using System.ComponentModel.DataAnnotations;

namespace Shared.Enums
{
    public enum BackendType
    {
        [Display(Name = ".NET", Description = ".NET")]
        DotNet = 1,

        [Display(Name = "Node.js", Description = "Node.js")]
        NodeJS = 2,

        [Display(Name = "Python", Description = "Python")]
        Python = 3,

        [Display(Name = "PHP", Description = "PHP")]
        PHP = 4,

        [Display(Name = "Other", Description = "أخرى")]
        Other = 5
    }
}
