
namespace Persistance.Data.Configurations.Product
{
    public class ProductConfiguration : IEntityTypeConfiguration<Domain.Entities.Product.Product>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Product.Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.NameAr)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.NameEn)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.DescriptionAr)
                .HasMaxLength(1000);

            builder.Property(e => e.DescriptionEn)
                .HasMaxLength(1000);

            builder.Property(e => e.BasePrice)
                .IsRequired()
                .HasPrecision(18, 2);
            builder.Property(e => e.ProductType)
                .IsRequired();

            builder.Property(e => e.Condition)
                .IsRequired();
            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(e => e.AllowRamUpgrade)
                .HasDefaultValue(false);

            builder.Property(e => e.AllowStorageUpgrade)
                .HasDefaultValue(false);

            builder.Property(e => e.AllowGpuUpgrade)
                .HasDefaultValue(false);
            builder.HasIndex(e => e.ProductType)
                .HasDatabaseName("IX_Products_ProductType");

            builder.HasIndex(e => e.IsActive)
                .HasDatabaseName("IX_Products_IsActive");

            builder.HasIndex(e => e.Condition)
                .HasDatabaseName("IX_Products_Condition");
            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.Property(e => e.CreatedAt)
    .IsRequired()
    .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);
        }
    }
}
