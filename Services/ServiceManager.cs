namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAdminService _adminService;
        private readonly IProductService _productService; 

        public ServiceManager(
            IAuthenticationService authenticationService,
            IAdminService adminService,
            IProductService productService) 
        {
            _authenticationService = authenticationService;
            _adminService = adminService;
            _productService = productService;
        }

        public IAuthenticationService AuthenticationService => _authenticationService;
        public IAdminService AdminService => _adminService;
        public IProductService ProductService => _productService;  
    }
}
