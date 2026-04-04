using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Data.Configurations.Product
{
    public class ProductMediaConfiguration : IEntityTypeConfiguration<ProductMedia>
    {
        public void Configure(EntityTypeBuilder<ProductMedia> builder)
        {
            builder.ToTable("ProductMedias");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.ProductId)
                .IsRequired();

            builder.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.MediaType)
                .IsRequired();

            builder.Property(e => e.Order)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.IsMain)
                .IsRequired()
                .HasDefaultValue(false);

            // Relationships
            builder.HasOne(e => e.Product)
                .WithMany(p => p.Media)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(e => e.ProductId)
                .HasDatabaseName("IX_ProductMedias_ProductId");

            builder.HasIndex(e => new { e.ProductId, e.IsMain })
                .HasDatabaseName("IX_ProductMedias_ProductId_IsMain");

            // Soft Delete
            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.Property(e => e.CreatedAt)
    .IsRequired()
    .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);
        }
    }
}
