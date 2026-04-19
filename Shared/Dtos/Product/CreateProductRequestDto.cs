namespace Shared.Dtos.Product
{
    public class CreateProductRequestDto
    {
        public int? UserId { get; set; }
        public Guid ProductId { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }
}