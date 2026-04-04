using System.ComponentModel.DataAnnotations;

namespace Shared.Enums
{
    public enum MediaType
    {
        [Display(Name = "Image", Description = "صورة")]
        Image = 1,

        [Display(Name = "Video", Description = "فيديو")]
        Video = 2
    }
}
