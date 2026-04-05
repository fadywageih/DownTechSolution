
namespace Persistance.Repositories
{
    public class IssueRepository : GenericRepository<Issue, Guid>, IIssueRepository
    {
        private readonly ApplicationDbContext _context;

        public IssueRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Issue>> GetIssuesByUserIdAsync(int userId) 
        {
            return await _context.Set<Issue>()
                .Include(i => i.User)
                .Include(i => i.Admin)
                .Where(i => i.UserId == userId && !i.IsDeleted)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Issue>> GetIssuesByAdminIdAsync(Guid adminId)
        {
            return await _context.Set<Issue>()
                .Include(i => i.User)
                .Include(i => i.Admin)
                .Where(i => i.AdminId == adminId && !i.IsDeleted)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Issue>> GetIssuesWithDetailsAsync(IssueFilterDto? filter = null)
        {
            var query = _context.Set<Issue>()
                .Include(i => i.User)
                .Include(i => i.Admin)
                .Where(i => !i.IsDeleted)
                .AsQueryable();

            if (filter != null)
            {
                if (filter.ProductType.HasValue)
                    query = query.Where(i => i.ProductType == filter.ProductType.Value);

                if (filter.Status.HasValue)
                    query = query.Where(i => i.Status == filter.Status.Value);

                if (filter.FromDate.HasValue)
                    query = query.Where(i => i.CreatedAt >= filter.FromDate.Value);

                if (filter.ToDate.HasValue)
                    query = query.Where(i => i.CreatedAt <= filter.ToDate.Value);

                if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                {
                    var search = filter.SearchTerm.ToLower();
                    query = query.Where(i =>
                        i.Model.ToLower().Contains(search) ||
                        i.CustomerName.ToLower().Contains(search) ||
                        i.PhoneNumber.Contains(search) ||
                        i.Description.ToLower().Contains(search));
                }

                query = query.Skip((filter.PageNumber - 1) * filter.PageSize)
                           .Take(filter.PageSize);
            }

            return await query.OrderByDescending(i => i.CreatedAt).ToListAsync();
        }

        public async Task<Issue?> GetIssueWithDetailsByIdAsync(Guid id)
        {
            return await _context.Set<Issue>()
                .Include(i => i.User)
                .Include(i => i.Admin)
                .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted);
        }

        public async Task<int> GetTotalIssuesCountAsync(IssueFilterDto? filter = null)
        {
            var query = _context.Set<Issue>().Where(i => !i.IsDeleted).AsQueryable();

            if (filter != null)
            {
                if (filter.ProductType.HasValue)
                    query = query.Where(i => i.ProductType == filter.ProductType.Value);

                if (filter.Status.HasValue)
                    query = query.Where(i => i.Status == filter.Status.Value);

                if (filter.FromDate.HasValue)
                    query = query.Where(i => i.CreatedAt >= filter.FromDate.Value);

                if (filter.ToDate.HasValue)
                    query = query.Where(i => i.CreatedAt <= filter.ToDate.Value);

                if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                {
                    var search = filter.SearchTerm.ToLower();
                    query = query.Where(i =>
                        i.Model.ToLower().Contains(search) ||
                        i.CustomerName.ToLower().Contains(search) ||
                        i.PhoneNumber.Contains(search));
                }
            }

            return await query.CountAsync();
        }

        public async Task<Dictionary<ProductType, int>> GetIssuesCountByProductTypeAsync()
        {
            return await _context.Set<Issue>()
                .Where(i => !i.IsDeleted)
                .GroupBy(i => i.ProductType)
                .Select(g => new { ProductType = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.ProductType, x => x.Count);
        }

        public async Task<double> GetAverageResolutionTimeAsync()
        {
            var resolvedIssues = await _context.Set<Issue>()
                .Where(i => i.ResolvedAt.HasValue && !i.IsDeleted)
                .ToListAsync();

            if (!resolvedIssues.Any())
                return 0;

            return resolvedIssues.Average(i =>
                (i.ResolvedAt!.Value - i.CreatedAt).TotalHours);
        }
    }
}
