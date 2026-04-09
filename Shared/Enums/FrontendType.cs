using System.ComponentModel.DataAnnotations;

namespace Shared.Enums
{
    public enum FrontendType
    {
        [Display(Name = "Angular", Description = "Angular")]
        Angular = 1,

        [Display(Name = "React", Description = "React")]
        React = 2,

        [Display(Name = "Vue", Description = "Vue.js")]
        Vue = 3,

        [Display(Name = "Vanilla JS", Description = "JavaScript خالص")]
        VanillaJS = 4,

        [Display(Name = "Other", Description = "أخرى")]
        Other = 5
    }
}
