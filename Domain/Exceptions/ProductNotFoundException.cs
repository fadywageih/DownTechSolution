namespace Domain.Exceptions
{
    public sealed class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid id)
            : base($"Product with ID {id} was not found.") { }

        public ProductNotFoundException(string name)
            : base($"Product with name '{name}' was not found.") { }
    }
}
