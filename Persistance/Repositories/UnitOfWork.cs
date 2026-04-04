using Domain.Contracts;
using Domain.Entities;
using Persistance.Data;
using System.Collections.Concurrent;

namespace Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private ConcurrentDictionary<string, object> _repositories;
        private IProductRepository? _productRepository;
        private IUpgradeOptionRepository? _upgradeOptionRepository;
        private IProductUpgradeRepository? _productUpgradeRepository;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new();
        }

        public IProductRepository ProductRepository =>
            _productRepository ??= new ProductRepository(_dbContext);

        public IUpgradeOptionRepository UpgradeOptionRepository =>
            _upgradeOptionRepository ??= new UpgradeOptionRepository(_dbContext);

        public IProductUpgradeRepository ProductUpgradeRepository =>
            _productUpgradeRepository ??= new ProductUpgradeRepository(_dbContext);

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            return (IGenericRepository<TEntity, TKey>)_repositories.GetOrAdd(typeof(TEntity).Name
                , (_) => new GenericRepository<TEntity, TKey>(_dbContext));
        }

        public Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();
    }
}
