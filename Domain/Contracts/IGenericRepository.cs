using Domain.Entities;

namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetQuery();
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
