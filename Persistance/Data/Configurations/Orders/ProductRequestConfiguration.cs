namespace Persistance.Data.Configurations.Orders
{
public class ProductRequestConfiguration : IEntityTypeConfiguration<ProductRequest>
    {
        public void Configure(EntityTypeBuilder<ProductRequest> builder)
        {
            builder.ToTable("ProductRequests");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            builder.Property(e => e.Details)
                .HasMaxLength(500);
            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            builder.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
