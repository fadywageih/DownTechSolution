using AutoMapper;
using Domain.Entities.Orders;
using Shared.Dtos.Product;

namespace Services.MappingProfile
{
    public class ProductRequestProfile : Profile
    {
        public ProductRequestProfile()
        {
            CreateMap<ProductRequest, ProductRequestDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.FirstName + (src.User.LastName != null ? " " + src.User.LastName : "") : "Guest"))
                .ForMember(dest => dest.ProductNameAr, opt => opt.MapFrom(src => src.Product != null ? src.Product.NameAr : ""))
                .ForMember(dest => dest.ProductNameEn, opt => opt.MapFrom(src => src.Product != null ? src.Product.NameEn : ""));
            CreateMap<CreateProductRequestDto, ProductRequest>();
        }
    }
}

