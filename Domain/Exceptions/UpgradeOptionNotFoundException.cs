namespace Domain.Exceptions
{
    public sealed class UpgradeOptionNotFoundException : NotFoundException
    {
        public UpgradeOptionNotFoundException(Guid id)
            : base($"Upgrade option with ID {id} was not found.") { }

        public UpgradeOptionNotFoundException(string name)
            : base($"Upgrade option with name '{name}' was not found.") { }
    }
}
