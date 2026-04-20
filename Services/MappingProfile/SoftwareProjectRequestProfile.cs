using AutoMapper;
using Domain.Entities.Orders;
using Shared.Dtos.SoftwareProject;

namespace Services.MappingProfile
{
    public class SoftwareProjectRequestProfile : Profile
    {
        public SoftwareProjectRequestProfile()
        {
            CreateMap<CreateSoftwareProjectRequestDto, SoftwareProjectRequest>();

            CreateMap<SoftwareProjectRequest, SoftwareProjectRequestDto>()
                .ForMember(dest => dest.SoftwareProjectNameEn,
                    opt => opt.MapFrom(src => src.SoftwareProject != null ? src.SoftwareProject.NameEn : ""))
                .ForMember(dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src => src.PhoneNumber)) // ✅ أضف السطر ده
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.UserEmail))
                .ForMember(dest => dest.SoftwareProjectId, opt => opt.MapFrom(src => src.SoftwareProjectId))
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
