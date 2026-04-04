namespace Domain.Exceptions
{
    public sealed class ProductAlreadyExistsException : Exception
    {
        public ProductAlreadyExistsException(string nameAr, string nameEn)
            : base($"Product with name '{nameAr}' / '{nameEn}' already exists.") { }

        public ProductAlreadyExistsException(Guid id)
            : base($"Product with ID {id} already exists.") { }
    }
}
