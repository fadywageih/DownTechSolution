namespace Domain.Exceptions
{
    public sealed class MediaNotFoundException : NotFoundException
    {
        public MediaNotFoundException(Guid id)
            : base($"Media with ID {id} was not found.") { }
    }
}
