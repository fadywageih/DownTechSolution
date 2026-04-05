namespace DownTech.Extentions
{
    public static class InfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrasturctureServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            Services.AddIdentityCore<User>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            Services.AddScoped<IAdminRepository, AdminRepository>();
            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddScoped<IUpgradeOptionRepository, UpgradeOptionRepository>();
            Services.AddScoped<IProductUpgradeRepository, ProductUpgradeRepository>();
            Services.AddScoped<IIssueRepository, IssueRepository>();

            Services.AddScoped<IUnitOfWork, UnitOfWork>();

            Services.AddHttpContextAccessor();


            Services.ConfigureJWT(Configuration);

            return Services;
        }
        public static IServiceCollection ConfigureJWT(this IServiceCollection Services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            // Disable automatic claim type mapping to preserve custom claim names
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                    NameClaimType = JwtRegisteredClaimNames.NameId,
                    RoleClaimType = ClaimTypes.Role
                };
            });
            Services.AddAuthorization();
            return Services;
        }
    }

}
