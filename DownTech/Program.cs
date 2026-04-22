namespace DownTech
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }

            #region Services
            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddPressentionServices();
            builder.Services.AddCoreServices(builder.Configuration);
            builder.Services.AddInfrasturctureServices(builder.Configuration);
            builder.Services.AddScoped<AdminSeeder>();
            #endregion

            var app = builder.Build();
            await app.ApplyMigrationsAsync();

            app.UseCustomMiddleWare();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            await app.SeedDbAsync();

            app.Run();
        }
    }
}