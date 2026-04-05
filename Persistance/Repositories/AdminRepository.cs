
namespace Persistance.Repositories
{
    public class AdminRepository : GenericRepository<Admin, Guid>, IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Admin?> GetByEmailAsync(string email)
        {
            return await _context.Set<Admin>()
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        public bool VerifyPassword(Admin admin, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash);
        }
    }
}
