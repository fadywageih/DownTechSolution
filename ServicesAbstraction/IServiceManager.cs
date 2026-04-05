namespace ServicesAbstraction
{
    public interface IServiceManager
    {
        public IAuthenticationService AuthenticationService { get; }
        public IAdminService AdminService { get; }
        public IProductService ProductService { get; }
        public IIssueService IssueService { get; }
    }
}
