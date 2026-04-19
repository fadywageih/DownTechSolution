using Domain.Entities;
using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Orders
{
    public class ProductRequest : BaseEntity<Guid>
    {
        public int? UserId { get; set; }
        public Guid ProductId { get; set; }
        [Required, MaxLength(20)]
        public string Phone { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Contacted, Sold, Rejected
        [MaxLength(500)]
        public string Details { get; set; } = string.Empty; // product name + upgrades summary

        // Nav props
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
        public Product.Product? Product { get; set; }
    }
}

