namespace Persistance.Data.Configurations.Product
{
    public class UpgradeOptionConfiguration : IEntityTypeConfiguration<UpgradeOption>
    {
        public void Configure(EntityTypeBuilder<UpgradeOption> builder)
        {
            builder.ToTable("UpgradeOptions");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.UpgradeType)
                .IsRequired();

            builder.Property(e => e.NameEn)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.NameAr)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(e => e.ApplicableTo)
                .IsRequired(false);

            // Indexes
            builder.HasIndex(e => e.UpgradeType)
                .HasDatabaseName("IX_UpgradeOptions_UpgradeType");

            builder.HasIndex(e => e.ApplicableTo)
                .HasDatabaseName("IX_UpgradeOptions_ApplicableTo");

            // Soft Delete
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }

}
