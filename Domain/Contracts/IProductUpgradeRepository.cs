using Domain.Entities.Product;

namespace Domain.Contracts
{
    public interface IProductUpgradeRepository : IGenericRepository<ProductUpgrade, Guid>
    {
        Task<IReadOnlyList<ProductUpgrade>> GetActiveUpgradesForProductAsync(Guid productId);
        Task<ProductUpgrade?> GetByUpgradeOptionAndProductAsync(Guid productId, Guid upgradeOptionId);
        Task<bool> HasActiveUpgradesAsync(Guid productId);
    }
}
