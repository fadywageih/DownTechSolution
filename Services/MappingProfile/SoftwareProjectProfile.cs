namespace Services.MappingProfile
{
    public class SoftwareProjectProfile : Profile
    {
        public SoftwareProjectProfile()
        {
            CreateMap<SoftwareProjectCreateDto, SoftwareProject>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.FrontendLibrariesJson, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByAdminId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedByAdminId, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());
            CreateMap<SoftwareProjectUpdateDto, SoftwareProject>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.FrontendLibrariesJson, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedByAdminId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByAdminId, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());
            CreateMap<SoftwareProject, SoftwareProjectResponseDto>()
                .ForMember(dest => dest.FrontendType,
                    opt => opt.MapFrom(src => GetFrontendTypeName(src.FrontendType)))
                .ForMember(dest => dest.FrontendTypeValue,
                    opt => opt.MapFrom(src => (int)src.FrontendType))
                .ForMember(dest => dest.FrontendLibraries,
                    opt => opt.MapFrom(src => GetFrontendLibraryNames(src.GetFrontendLibraries())))
                .ForMember(dest => dest.BackendType,
                    opt => opt.MapFrom(src => GetBackendTypeName(src.BackendType)))
                .ForMember(dest => dest.BackendTypeValue,
                    opt => opt.MapFrom(src => (int)src.BackendType))
                .ForMember(dest => dest.BackendFramework,
                    opt => opt.MapFrom(src => src.BackendFramework.HasValue ? GetBackendFrameworkName(src.BackendFramework.Value) : null))
                .ForMember(dest => dest.BackendFrameworkValue,
                    opt => opt.MapFrom(src => (int?)src.BackendFramework))
                .ForMember(dest => dest.Database,
                    opt => opt.MapFrom(src => src.Database.HasValue ? GetDatabaseName(src.Database.Value) : null))
                .ForMember(dest => dest.DatabaseValue,
                    opt => opt.MapFrom(src => (int?)src.Database));
            CreateMap<SoftwareProject, SoftwareProjectListDto>()
                .ForMember(dest => dest.FrontendType,
                    opt => opt.MapFrom(src => GetFrontendTypeName(src.FrontendType)))
                .ForMember(dest => dest.FrontendTypeValue,
                    opt => opt.MapFrom(src => (int)src.FrontendType))
                .ForMember(dest => dest.BackendType,
                    opt => opt.MapFrom(src => GetBackendTypeName(src.BackendType)))
                .ForMember(dest => dest.BackendTypeValue,
                    opt => opt.MapFrom(src => (int)src.BackendType))
                .ForMember(dest => dest.BackendFramework,
                    opt => opt.MapFrom(src => src.BackendFramework.HasValue ? GetBackendFrameworkName(src.BackendFramework.Value) : null))
                .ForMember(dest => dest.BackendFrameworkValue,
                    opt => opt.MapFrom(src => (int?)src.BackendFramework))
                .ForMember(dest => dest.Database,
                    opt => opt.MapFrom(src => src.Database.HasValue ? GetDatabaseName(src.Database.Value) : null))
                .ForMember(dest => dest.DatabaseValue,
                    opt => opt.MapFrom(src => (int?)src.Database));
        }

        private static string GetFrontendTypeName(FrontendType type)
        {
            return type switch
            {
                FrontendType.Angular => "Angular",
                FrontendType.React => "React",
                FrontendType.Vue => "Vue.js",
                FrontendType.VanillaJS => "Vanilla JS",
                FrontendType.Other => "Other",
                _ => type.ToString()
            };
        }

        private static string GetBackendTypeName(BackendType type)
        {
            return type switch
            {
                BackendType.DotNet => ".NET",
                BackendType.NodeJS => "Node.js",
                BackendType.Python => "Python",
                BackendType.PHP => "PHP",
                BackendType.Other => "Other",
                _ => type.ToString()
            };
        }

        private static string GetBackendFrameworkName(BackendFramework framework)
        {
            return framework switch
            {
                BackendFramework.AspNetCore => "ASP.NET Core",
                BackendFramework.ExpressJS => "Express.js",
                BackendFramework.Django => "Django",
                BackendFramework.Flask => "Flask",
                BackendFramework.Laravel => "Laravel",
                BackendFramework.None => "None",
                BackendFramework.Other => "Other",
                _ => framework.ToString()
            };
        }

        private static string GetDatabaseName(DatabaseType database)
        {
            return database switch
            {
                DatabaseType.SqlServer => "SQL Server",
                DatabaseType.MySql => "MySQL",
                DatabaseType.PostgreSQL => "PostgreSQL",
                DatabaseType.MongoDB => "MongoDB",
                DatabaseType.None => "None",
                DatabaseType.Other => "Other",
                _ => database.ToString()
            };
        }

        private static List<string> GetFrontendLibraryNames(List<FrontendLibrary> libraries)
        {
            return libraries.Select(library => library switch
            {
                FrontendLibrary.Tailwind => "Tailwind CSS",
                FrontendLibrary.Bootstrap => "Bootstrap",
                FrontendLibrary.MaterialUI => "Material UI",
                FrontendLibrary.AntDesign => "Ant Design",
                FrontendLibrary.ChakraUI => "Chakra UI",
                FrontendLibrary.None => "None",
                FrontendLibrary.Other => "Other",
                _ => library.ToString()
            }).ToList();
        }
    }
}
