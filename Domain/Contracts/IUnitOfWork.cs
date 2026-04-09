using Domain.Entities;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
        IProductRepository ProductRepository { get; }
        IUpgradeOptionRepository UpgradeOptionRepository { get; }
        IProductUpgradeRepository ProductUpgradeRepository { get; }
        ISoftwareProjectRepository SoftwareProjectRepository { get; }
    }
}
