using Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Product
{
    public class ProductMedia : BaseEntity<Guid>
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required(ErrorMessage = "Media URL is required")]
        [MaxLength(500, ErrorMessage = "URL cannot exceed 500 characters")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string Url { get; set; } = string.Empty;
        [Required]
        public MediaType MediaType { get; set; }
        [Range(0, 100, ErrorMessage = "Order must be between 0 and 100")]
        public int Order { get; set; }

        public bool IsMain { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
