namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAdminService _adminService;
        private readonly IProductService _productService;
        private readonly IIssueService _IssueService;
        private readonly ISoftwareProjectService _softwareProjectService;
        public ServiceManager(
            IAuthenticationService authenticationService,
            IAdminService adminService,
            IProductService productService,
            IIssueService issueService,
            ISoftwareProjectService softwareProjectService) 
        {
            _authenticationService = authenticationService;
            _adminService = adminService;
            _productService = productService;
            _IssueService= issueService;
            _softwareProjectService = softwareProjectService;
        }

        public IAuthenticationService AuthenticationService => _authenticationService;
        public IAdminService AdminService => _adminService;
        public IProductService ProductService => _productService;
        public IIssueService IssueService => _IssueService;
        public ISoftwareProjectService SoftwareProjectService => _softwareProjectService;
    }
}
