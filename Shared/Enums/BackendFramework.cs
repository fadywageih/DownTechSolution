using System.ComponentModel.DataAnnotations;

namespace Shared.Enums
{
    public enum BackendFramework
    {
        [Display(Name = "ASP.NET Core", Description = "ASP.NET Core")]
        AspNetCore = 1,

        [Display(Name = "Express.js", Description = "Express.js")]
        ExpressJS = 2,

        [Display(Name = "Django", Description = "Django")]
        Django = 3,

        [Display(Name = "Flask", Description = "Flask")]
        Flask = 4,

        [Display(Name = "Laravel", Description = "Laravel")]
        Laravel = 5,

        [Display(Name = "None", Description = "بدون إطار عمل")]
        None = 6,

        [Display(Name = "Other", Description = "أخرى")]
        Other = 7
    }
}
