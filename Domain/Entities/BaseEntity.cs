namespace Domain.Entities
{
    public class BaseEntity<Tkey>
    {
        public Tkey Id { get; set; }
        public bool IsDeleted { get; set; } = false; 
        public DateTime? DeletedAt { get; set; }
    }
}
