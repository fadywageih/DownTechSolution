using System.ComponentModel.DataAnnotations;

namespace Shared.Enums
{
    public enum IssueStatus
    {
        [Display(Name = "Pending", Description = "قيد الانتظار")]
        Pending = 1,

        [Display(Name = "In Progress", Description = "قيد المعالجة")]
        InProgress = 2,

        [Display(Name = "Resolved", Description = "تم الحل")]
        Resolved = 3,

        [Display(Name = "Closed", Description = "مغلق")]
        Closed = 4
    }
}
