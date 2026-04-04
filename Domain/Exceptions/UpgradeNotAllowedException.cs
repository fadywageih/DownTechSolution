namespace Domain.Exceptions
{
    public sealed class UpgradeNotAllowedException : Exception
    {
        public UpgradeNotAllowedException(string upgradeType, Guid productId)
            : base($"Upgrade of type '{upgradeType}' is not allowed for product with ID {productId}.") { }
    }
}
