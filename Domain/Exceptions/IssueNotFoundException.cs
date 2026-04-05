namespace Domain.Exceptions
{
    public sealed class IssueNotFoundException : NotFoundException
    {
        public IssueNotFoundException(Guid id)
            : base($"Issue with ID {id} was not found.") { }
    }
}
