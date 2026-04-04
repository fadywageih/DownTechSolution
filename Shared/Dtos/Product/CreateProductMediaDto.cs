using Shared.Enums;

namespace Shared.Dtos.Product
{
    public class CreateProductMediaDto
    {
        public string Url { get; set; } = string.Empty;
        public MediaType MediaType { get; set; }
        public int Order { get; set; }
        public bool IsMain { get; set; }
    }
}
