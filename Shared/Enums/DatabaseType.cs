using System.ComponentModel.DataAnnotations;

namespace Shared.Enums
{
    public enum DatabaseType
    {
        [Display(Name = "SQL Server", Description = "SQL Server")]
        SqlServer = 1,

        [Display(Name = "MySQL", Description = "MySQL")]
        MySql = 2,

        [Display(Name = "PostgreSQL", Description = "PostgreSQL")]
        PostgreSQL = 3,

        [Display(Name = "MongoDB", Description = "MongoDB")]
        MongoDB = 4,

        [Display(Name = "None", Description = "بدون قاعدة بيانات")]
        None = 5,

        [Display(Name = "Other", Description = "أخرى")]
        Other = 6
    }
}
