using Domain.Entities.Product;
using Shared.Enums;

namespace Domain.Contracts
{
    public interface IUpgradeOptionRepository : IGenericRepository<UpgradeOption, Guid>
    {
        Task<IReadOnlyList<UpgradeOption>> GetUpgradesByTypeAsync(UpgradeType upgradeType);
        Task<IReadOnlyList<UpgradeOption>> GetUpgradesApplicableToAsync(ProductType? productType);
        Task<UpgradeOption?> GetByNameAsync(string name);
    }
}
