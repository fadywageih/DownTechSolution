namespace Services.MappingProfile
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.AvailableUpgrades,
                    opt => opt.MapFrom(src => src.ProductUpgrades))
                .ForMember(dest => dest.AccessoryType,
                    opt => opt.MapFrom(src => MapAccessoryType(src)));
            CreateMap<Accessory, ProductDto>()
                .IncludeBase<Product, ProductDto>();
            CreateMap<ProductSpecification, ProductSpecificationDto>();
            CreateMap<ProductMedia, ProductMediaDto>();
            CreateMap<ProductUpgrade, ProductUpgradeDto>()
                .ForMember(dest => dest.NameAr,
                    opt => opt.MapFrom(src => src.UpgradeOption != null ? src.UpgradeOption.NameAr : string.Empty))
                .ForMember(dest => dest.NameEn,
                    opt => opt.MapFrom(src => src.UpgradeOption != null ? src.UpgradeOption.NameEn : string.Empty));
            CreateMap<UpgradeOption, ProductUpgradeDto>()
                .ForMember(dest => dest.UpgradeOptionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FromValue, opt => opt.MapFrom(src => string.Empty))
                .ForMember(dest => dest.ToValue, opt => opt.MapFrom(src => string.Empty))
                .ForMember(dest => dest.AdditionalPrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn));
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Specifications, opt => opt.Ignore())
                .ForMember(dest => dest.Media, opt => opt.Ignore())
                .ForMember(dest => dest.ProductUpgrades, opt => opt.Ignore())
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType))
                .ForMember(dest => dest.AllowRamUpgrade, opt => opt.MapFrom(src => src.AllowRamUpgrade))
                .ForMember(dest => dest.AllowStorageUpgrade, opt => opt.MapFrom(src => src.AllowStorageUpgrade))
                .ForMember(dest => dest.AllowGpuUpgrade, opt => opt.MapFrom(src => src.AllowGpuUpgrade));
            CreateMap<CreateProductDto, Accessory>()
                .IncludeBase<CreateProductDto, Product>() 
                .ForMember(dest => dest.AccessoryType, opt => opt.MapFrom(src => src.AccessoryType ?? AccessoryType.Monitor));
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Specifications, opt => opt.Ignore())
                .ForMember(dest => dest.Media, opt => opt.Ignore())
                .ForMember(dest => dest.ProductUpgrades, opt => opt.Ignore());
            CreateMap<UpdateProductDto, Accessory>()
                .IncludeBase<UpdateProductDto, Product>()
                .ForMember(dest => dest.AccessoryType, opt => opt.MapFrom(src => src.AccessoryType ?? AccessoryType.Monitor));
            CreateMap<CreateProductSpecificationDto, ProductSpecification>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<CreateProductMediaDto, ProductMedia>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<UpdateProductSpecificationDto, ProductSpecification>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.Ignore());
            CreateMap<UpdateProductMediaDto, ProductMedia>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.Ignore());
        }
        private static AccessoryType? MapAccessoryType(Product product)
        {
            return product is Accessory accessory ? accessory.AccessoryType : null;
        }
    }
}