namespace Shared.Dtos.Product
{
    public abstract class BaseDto<TKey>
    {
        public TKey Id { get; set; } = default!;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
