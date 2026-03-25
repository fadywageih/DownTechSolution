namespace DownTech.Extentions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<AdminSeeder>();
            await seeder.SeedSuperAdminAsync();
            return app;
        }
        public static WebApplication UseCustomMiddleWare(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleWare>();
            return app;
        }
    }
}
