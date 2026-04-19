namespace Persistance.Repositories
{
    public class ProductRepository : GenericRepository<Product, Guid>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Product?> GetProductWithDetailsAsync(Guid id)
        {
            return await _context.Set<Product>()
                .Include(p => p.Specifications
                    .Where(s => !s.IsDeleted)
                    .OrderBy(s => s.CreatedAt))
                .Include(p => p.Media
                    .Where(m => !m.IsDeleted)
                    .OrderBy(m => m.Order))
                .Include(p => p.ProductUpgrades
                    .Where(pu => pu.IsActive && !pu.IsDeleted)
                    .OrderBy(pu => pu.Id)) 
                        .ThenInclude(pu => pu.UpgradeOption)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<IReadOnlyList<Product>> GetProductsByTypeAsync(ProductType productType)
        {
            return await _context.Set<Product>()
                .Where(p => p.ProductType == productType && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProductsByConditionAsync(DeviceCondition condition)
        {
            return await _context.Set<Product>()
                .Where(p => p.Condition == condition && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProductsWithUpgradesAsync()
        {
            return await _context.Set<Product>()
                .Include(p => p.ProductUpgrades.Where(pu => pu.IsActive))
                    .ThenInclude(pu => pu.UpgradeOption)
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Product?> GetByNamesAsync(string nameAr, string nameEn)
        {
            return await _context.Set<Product>()
                .FirstOrDefaultAsync(p => (p.NameAr == nameAr || p.NameEn == nameEn) && !p.IsDeleted);
        }

        public async Task<IReadOnlyList<Product>> GetActiveProductsAsync()
        {
            return await _context.Set<Product>()
                .Where(p => p.IsActive && !p.IsDeleted)
                .ToListAsync();
        }

public async Task<IReadOnlyList<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Set<Product>()
                .Where(p => p.BasePrice >= minPrice && p.BasePrice <= maxPrice && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Orders.ProductRequest>> GetProductRequestsAsync()
        {
            return await _context.ProductRequests
                .Include(pr => pr.Product)
                    .ThenInclude(p => p!.Media.Where(m => m.IsMain))
                .Include(pr => pr.User)
                .Where(pr => !pr.IsDeleted)
                .OrderByDescending(pr => pr.CreatedAt)
                .ToListAsync();
        }
    }
}
