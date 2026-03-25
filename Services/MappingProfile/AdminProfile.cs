namespace Services.MappingProfile
{
    public class AdminProfile: Profile
    {
        
            public AdminProfile()
            {
                CreateMap<Admin, AdminResultDto>();
                CreateMap<AdminCreateDto, Admin>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                    .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            }
    }
}
