namespace Persistance.Repositories
{
    public class SoftwareProjectRepository : GenericRepository<SoftwareProject, Guid>, ISoftwareProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public SoftwareProjectRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SoftwareProject>> GetSoftwareProjectsWithFilterAsync(SoftwareProjectFilterDto? filter = null)
        {
            var query = _context.Set<SoftwareProject>()
                .Where(p => !p.IsDeleted)
                .AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                {
                    var search = filter.SearchTerm.ToLower();
                    query = query.Where(p =>
                        p.NameEn.ToLower().Contains(search) ||
                        p.NameAr.Contains(search) ||
                        p.DescriptionEn.ToLower().Contains(search) ||
                        p.DescriptionAr.Contains(search));
                }

                if (filter.FrontendType.HasValue)
                {
                    query = query.Where(p => p.FrontendType == filter.FrontendType.Value);
                }

                if (filter.BackendType.HasValue)
                {
                    query = query.Where(p => p.BackendType == filter.BackendType.Value);
                }

                // ✅ الآن PageNumber و PageSize من نوع int?،所以可以直接 استخدام Value
                if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
                {
                    query = query.Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value)
                               .Take(filter.PageSize.Value);
                }
            }

            // ✅ إضافة OrderBy
            query = query.OrderByDescending(p => p.CreatedAt);

            return await query.ToListAsync();
        }

        public async Task<SoftwareProject?> GetSoftwareProjectWithDetailsAsync(Guid id)
        {
            return await _context.Set<SoftwareProject>()
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<IEnumerable<SoftwareProject>> GetLatestProjectsAsync(int count)
        {
            return await _context.Set<SoftwareProject>()
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> IsNameExistsAsync(string nameAr, string nameEn, Guid? excludeId = null)
        {
            var query = _context.Set<SoftwareProject>()
                .Where(p => !p.IsDeleted && (p.NameAr == nameAr || p.NameEn == nameEn));

            if (excludeId.HasValue)
            {
                query = query.Where(p => p.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<int> GetTotalCountAsync(SoftwareProjectFilterDto? filter = null)
        {
            var query = _context.Set<SoftwareProject>()
                .Where(p => !p.IsDeleted)
                .AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                {
                    var search = filter.SearchTerm.ToLower();
                    query = query.Where(p =>
                        p.NameEn.ToLower().Contains(search) ||
                        p.NameAr.Contains(search) ||
                        p.DescriptionEn.ToLower().Contains(search) ||
                        p.DescriptionAr.Contains(search));
                }

                if (filter.FrontendType.HasValue)
                {
                    query = query.Where(p => p.FrontendType == filter.FrontendType.Value);
                }

                if (filter.BackendType.HasValue)
                {
                    query = query.Where(p => p.BackendType == filter.BackendType.Value);
                }
            }

            return await query.CountAsync();
        }

        public async Task<Dictionary<FrontendType, int>> GetProjectsCountByFrontendTypeAsync()
        {
            return await _context.Set<SoftwareProject>()
                .Where(p => !p.IsDeleted)
                .GroupBy(p => p.FrontendType)
                .Select(g => new { FrontendType = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.FrontendType, x => x.Count);
        }

        public async Task<Dictionary<BackendType, int>> GetProjectsCountByBackendTypeAsync()
        {
            return await _context.Set<SoftwareProject>()
                .Where(p => !p.IsDeleted)
                .GroupBy(p => p.BackendType)
                .Select(g => new { BackendType = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.BackendType, x => x.Count);
        }
    }
}