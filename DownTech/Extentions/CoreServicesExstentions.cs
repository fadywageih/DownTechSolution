namespace DownTech.Extentions
{
    public static class CoreServicesExstentions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection Services, IConfiguration configuration)
        {
            Services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

            Services.AddScoped<IServiceManager, ServiceManager>();
            Services.AddScoped<IEmailService, EmailService>();
            Services.AddScoped<IAdminService, AdminService>();
            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<IAuthenticationService,AuthenticationService>();
            Services.AddScoped<IIssueService, IssueService>();

            Services.AddScoped<IExtendedImageService, ImageService>();
            Services.AddScoped<IImageService>(provider => provider.GetRequiredService<IExtendedImageService>());
            Services.AddAutoMapper(typeof(AssemblyReference).Assembly);
            Services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            return Services;
        }
    }
}
