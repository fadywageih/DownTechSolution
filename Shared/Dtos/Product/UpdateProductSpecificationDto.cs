using System;

namespace Shared.Dtos.Product
{
    public class UpdateProductSpecificationDto : CreateProductSpecificationDto
    {
        public Guid? Id { get; set; }
    }
}

