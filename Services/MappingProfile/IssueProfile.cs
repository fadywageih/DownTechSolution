namespace Services.MappingProfile
{
    public class IssueProfile : Profile
    {
        public IssueProfile()
        {
            CreateMap<IssueCreateDto, Issue>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<Issue, IssueResponseDto>()
                .ForMember(dest => dest.ProductTypeName,
                    opt => opt.MapFrom(src => GetProductTypeName(src.ProductType)))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => GetStatusDescription(src.Status)))
                .ForMember(dest => dest.StatusValue,
                    opt => opt.MapFrom(src => (int)src.Status))
                .ForMember(dest => dest.UserEmail,
                    opt => opt.MapFrom(src => src.User != null ? src.User.Email : null))
                .ForMember(dest => dest.AssignedAdminName,
                    opt => opt.MapFrom(src => src.Admin != null ? $"{src.Admin.FirstName} {src.Admin.LastName}" : null));
        }

        private static string GetProductTypeName(ProductType type)
        {
            return type switch
            {
                ProductType.Laptop => "لابتوب",
                ProductType.PC => "بي سي",
                ProductType.Accessory => "إكسسوار",
                _ => type.ToString()
            };
        }

        private static string GetStatusDescription(IssueStatus status)
        {
            return status switch
            {
                IssueStatus.Pending => "قيد الانتظار",
                IssueStatus.InProgress => "قيد المعالجة",
                IssueStatus.Resolved => "تم الحل",
                IssueStatus.Closed => "مغلق",
                _ => status.ToString()
            };
        }
    }
}
