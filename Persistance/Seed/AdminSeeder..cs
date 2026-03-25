
using Domain.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Persistance.Data;

namespace Persistence.Seed
{
    public class AdminSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminSeeder> _logger;
        private readonly IConfiguration _configuration;

        public AdminSeeder(ApplicationDbContext context, ILogger<AdminSeeder> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SeedSuperAdminAsync()
        {
            try
            {
                var adminEmail = _configuration["SuperAdmin:Email"] ?? "superadmin@downtech.com";
                var adminPassword = _configuration["SuperAdmin:Password"] ?? "SuperAdmin@123";
                var existingAdmin = await _context.Admins
                    .FirstOrDefaultAsync(a => a.Email == adminEmail);

                if (existingAdmin == null)
                {
                    var superAdmin = new Admin
                    {
                        Id = Guid.NewGuid(),
                        Email = adminEmail,
                        FirstName = "Super",
                        LastName = "Admin",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword, workFactor: 12),
                        Role = "SuperAdmin",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.Admins.AddAsync(superAdmin);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    if (!BCrypt.Net.BCrypt.Verify(adminPassword, existingAdmin.PasswordHash))
                    {
                        existingAdmin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword, workFactor: 12);
                        await _context.SaveChangesAsync();
                    }

                    if (existingAdmin.Email != adminEmail)
                    {
                        existingAdmin.Email = adminEmail;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding SuperAdmin");
            }
        }
    }
}