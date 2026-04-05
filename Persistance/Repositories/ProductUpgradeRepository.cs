namespace Persistance.Repositories
{
    public class ProductUpgradeRepository : GenericRepository<ProductUpgrade, Guid>, IProductUpgradeRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductUpgradeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProductUpgrade>> GetActiveUpgradesForProductAsync(Guid productId)
        {
            return await _context.Set<ProductUpgrade>()
                .Include(pu => pu.UpgradeOption)
                .Where(pu => pu.ProductId == productId && pu.IsActive && !pu.IsDeleted)
                .ToListAsync();
        }

        public async Task<ProductUpgrade?> GetByUpgradeOptionAndProductAsync(Guid productId, Guid upgradeOptionId)
        {
            return await _context.Set<ProductUpgrade>()
                .FirstOrDefaultAsync(pu => pu.ProductId == productId &&
                                          pu.UpgradeOptionId == upgradeOptionId &&
                                          pu.IsActive &&
                                          !pu.IsDeleted);
        }

        public async Task<bool> HasActiveUpgradesAsync(Guid productId)
        {
            return await _context.Set<ProductUpgrade>()
                .AnyAsync(pu => pu.ProductId == productId && pu.IsActive && !pu.IsDeleted);
        }
    }
}
