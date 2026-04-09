namespace Persistance.Data.Configurations
{
    public class SoftwareProjectConfiguration : IEntityTypeConfiguration<SoftwareProject>
    {
        public void Configure(EntityTypeBuilder<SoftwareProject> builder)
        {
            builder.ToTable("SoftwareProjects");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.NameAr)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.DescriptionAr)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.DescriptionEn)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.FrontendType)
                .IsRequired();

            builder.Property(x => x.FrontendLibrariesJson)
                .HasMaxLength(1000);

            builder.Property(x => x.BackendType)
                .IsRequired();

            builder.Property(x => x.BackendFramework);

            builder.Property(x => x.Database);

            builder.Property(x => x.GithubUrl)
                .HasMaxLength(500);

            builder.Property(x => x.LiveDemoUrl)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.UpdatedAt)
                .IsRequired(false);

            // Relationships
            builder.HasOne(x => x.CreatedByAdmin)
                .WithMany()
                .HasForeignKey(x => x.CreatedByAdminId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UpdatedByAdmin)
                .WithMany()
                .HasForeignKey(x => x.UpdatedByAdminId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.NameAr)
                .HasDatabaseName("IX_SoftwareProjects_NameAr");

            builder.HasIndex(x => x.NameEn)
                .HasDatabaseName("IX_SoftwareProjects_NameEn");

            builder.HasIndex(x => x.FrontendType)
                .HasDatabaseName("IX_SoftwareProjects_FrontendType");

            builder.HasIndex(x => x.BackendType)
                .HasDatabaseName("IX_SoftwareProjects_BackendType");

            builder.HasIndex(x => x.CreatedAt)
                .HasDatabaseName("IX_SoftwareProjects_CreatedAt");

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
