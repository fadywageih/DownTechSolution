namespace Domain.Exceptions
{
    public class RequestNotFoundException : NotFoundException
    {
        public RequestNotFoundException(Guid id)
            : base($"Request with ID {id} not found")
        {
        }
        public RequestNotFoundException(string message)
            : base(message)
        {
        }
    }
}
