namespace Persistance.Repositories
{
    public class UpgradeOptionRepository : GenericRepository<UpgradeOption, Guid>, IUpgradeOptionRepository
    {
        private readonly ApplicationDbContext _context;

        public UpgradeOptionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<UpgradeOption>> GetUpgradesByTypeAsync(UpgradeType upgradeType)
        {
            return await _context.Set<UpgradeOption>()
                .Where(u => u.UpgradeType == upgradeType && !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<UpgradeOption>> GetUpgradesApplicableToAsync(ProductType? productType)
        {
            var query = _context.Set<UpgradeOption>().Where(u => !u.IsDeleted);

            if (productType.HasValue)
            {
                query = query.Where(u => u.ApplicableTo == null || u.ApplicableTo == productType);
            }

            return await query.ToListAsync();
        }

        public async Task<UpgradeOption?> GetByNameAsync(string name)
        {
            return await _context.Set<UpgradeOption>()
                .FirstOrDefaultAsync(u => u.NameEn == name || u.NameAr == name && !u.IsDeleted);
        }
    }
}
