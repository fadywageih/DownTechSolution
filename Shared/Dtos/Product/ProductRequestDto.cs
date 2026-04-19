using Shared.Dtos.Product;

namespace Shared.Dtos.Product
{
    public class ProductRequestDto
    {
        public Guid Id { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public Guid ProductId { get; set; }
        public string ProductNameAr { get; set; } = string.Empty;
        public string ProductNameEn { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public string Details { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

