using System;

namespace Shared.Dtos.Product
{
    public class UpdateProductMediaDto : CreateProductMediaDto
    {
        public Guid? Id { get; set; }
    }
}

