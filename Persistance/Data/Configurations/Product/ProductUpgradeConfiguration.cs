namespace Persistance.Data.Configurations.Product
{
    public class ProductUpgradeConfiguration : IEntityTypeConfiguration<ProductUpgrade>
    {
        public void Configure(EntityTypeBuilder<ProductUpgrade> builder)
        {
            builder.ToTable("ProductUpgrades");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.ProductId)
                .IsRequired();

            builder.Property(e => e.UpgradeOptionId)
                .IsRequired(false);

            builder.Property(e => e.UpgradeType)
                .IsRequired();

            builder.Property(e => e.FromValue)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.ToValue)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.AdditionalPrice)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Relationships
            builder.HasOne(e => e.Product)
                .WithMany(p => p.ProductUpgrades)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.UpgradeOption)
                .WithMany()
                .HasForeignKey(e => e.UpgradeOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(e => e.ProductId)
                .HasDatabaseName("IX_ProductUpgrades_ProductId");

            builder.HasIndex(e => e.IsActive)
                .HasDatabaseName("IX_ProductUpgrades_IsActive");

            builder.HasIndex(e => e.UpgradeType)
                .HasDatabaseName("IX_ProductUpgrades_UpgradeType");

            // Composite index for common queries
            builder.HasIndex(e => new { e.ProductId, e.IsActive, e.UpgradeType })
                .HasDatabaseName("IX_ProductUpgrades_ProductId_IsActive_UpgradeType");

            // Soft Delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
