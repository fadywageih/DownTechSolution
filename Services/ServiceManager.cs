namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAdminService _adminService;
        private readonly IProductService _productService;
        private readonly IIssueService _IssueService;

        public ServiceManager(
            IAuthenticationService authenticationService,
            IAdminService adminService,
            IProductService productService,
            IIssueService issueService) 
        {
            _authenticationService = authenticationService;
            _adminService = adminService;
            _productService = productService;
            _IssueService= issueService;
        }

        public IAuthenticationService AuthenticationService => _authenticationService;
        public IAdminService AdminService => _adminService;
        public IProductService ProductService => _productService;
        public IIssueService IssueService => _IssueService;
    }
}
