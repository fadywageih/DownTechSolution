namespace Domain.Exceptions
{
    public sealed class SoftwareProjectNotFoundException : NotFoundException
    {
        public SoftwareProjectNotFoundException(Guid id)
            : base($"Software project with ID {id} was not found.") { }

        public SoftwareProjectNotFoundException(string name)
            : base($"Software project with name '{name}' was not found.") { }
    }
}
    