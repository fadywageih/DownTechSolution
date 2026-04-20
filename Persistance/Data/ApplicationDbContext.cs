namespace Persistance.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public DbSet<ProductMedia> ProductMedias { get; set; }
        public DbSet<UpgradeOption> UpgradeOptions { get; set; }
        public DbSet<ProductUpgrade> ProductUpgrades { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<SoftwareProject> SoftwareProjects { get; set; }
        public DbSet<ProductRequest> ProductRequests { get; set; }
        public DbSet<SoftwareProjectRequest> SoftwareProjectRequests { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

}
