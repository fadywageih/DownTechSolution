namespace Persistance.Data.Configurations.Product
{
    public class ProductSpecificationConfiguration : IEntityTypeConfiguration<ProductSpecification>
    {
        public void Configure(EntityTypeBuilder<ProductSpecification> builder)
        {
            builder.ToTable("ProductSpecifications");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.ProductId)
                .IsRequired();
            builder.Property(e => e.KeyAr)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.KeyEn)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.ValueAr)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.ValueEn)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.IsUpgradable)
                .IsRequired()
                .HasDefaultValue(false);
            builder.HasOne(e => e.Product)
                .WithMany(p => p.Specifications)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(e => e.ProductId)
                .HasDatabaseName("IX_ProductSpecifications_ProductId");
            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.Property(e => e.CreatedAt)
    .IsRequired()
    .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);
        }
    }
}
