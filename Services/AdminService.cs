namespace Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AdminService> _logger;
        private readonly JwtOptions _jwtOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminService(
            IAdminRepository adminRepository,
            IUnitOfWork unitOfWork,
            ILogger<AdminService> logger,
            IOptions<JwtOptions> jwtOptions,
            IHttpContextAccessor httpContextAccessor)
        {
            _adminRepository = adminRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _jwtOptions = jwtOptions.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AdminAuthResultDto> LoginAsync(AdminLoginDto loginDto)
        {
            _logger.LogInformation("Admin login attempt: {Email}", loginDto.Email);

            var admin = await _adminRepository.GetByEmailAsync(loginDto.Email);
            if (admin?.LockoutEnd > DateTime.UtcNow)
            {
                throw new UnauthorizedException("Account is temporarily locked. Please try again later.");
            }

            if (admin == null || !admin.IsActive || !_adminRepository.VerifyPassword(admin, loginDto.Password))
            {
                if (admin != null)
                {
                    admin.FailedLoginAttempts++;
                    if (admin.FailedLoginAttempts >= 5)
                    {
                        admin.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                    }
                    _adminRepository.Update(admin);
                    await _unitOfWork.SaveChangesAsync();
                }

                _logger.LogWarning("Invalid login attempt for: {Email}", loginDto.Email);
                throw new UnauthorizedException("Invalid email or password");
            }
            admin.FailedLoginAttempts = 0;
            admin.LockoutEnd = null;
            admin.LastLogin = DateTime.UtcNow;
            var token = GenerateToken(admin);
            var refreshToken = GenerateRefreshToken();
            admin.RefreshToken = refreshToken;
            admin.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            _adminRepository.Update(admin);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Admin login successful: {Email}", admin.Email);
            return new AdminAuthResultDto
            {
                Id = admin.Id.ToString(),
                Email = admin.Email,
                FullName = $"{admin.FirstName} {admin.LastName}",
                Token = token,
                RefreshToken = refreshToken,
                Role = admin.Role,
                LastLogin = admin.LastLogin
            };
        }

        public async Task<AdminAuthResultDto> RegisterAsync(AdminCreateDto createDto)
        {
            _logger.LogInformation("Admin registration attempt: {Email}", createDto.Email);
            var currentAdminRole = GetCurrentAdminRole();
            if (currentAdminRole != "SuperAdmin")
            {
                throw new UnauthorizedException("Only Super Admin can create new admins");
            }
            if (await _adminRepository.GetByEmailAsync(createDto.Email) != null)
            {
                throw new AdminExistsException(createDto.Email);
            }
            var admin = new Admin
            {
                Id = Guid.NewGuid(),
                Email = createDto.Email,
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                PasswordHash = _adminRepository.HashPassword(createDto.Password),
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _adminRepository.AddAsync(admin);
            await _unitOfWork.SaveChangesAsync();
            var token = GenerateToken(admin);
            var refreshToken = GenerateRefreshToken();
            admin.RefreshToken = refreshToken;
            admin.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            _adminRepository.Update(admin);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Admin registered successfully: {Email}", admin.Email);
            return new AdminAuthResultDto
            {
                Id = admin.Id.ToString(),
                Email = admin.Email,
                FullName = $"{admin.FirstName} {admin.LastName}",
                Token = token,
                RefreshToken = refreshToken,
                Role = admin.Role,
                LastLogin = admin.LastLogin
            };
        }

        public async Task<AdminAuthResultDto> RefreshTokenAsync(string refreshToken)
        {
            var admins = await _adminRepository.GetAllAsync();
            var admin = admins.FirstOrDefault(a =>
                a.RefreshToken == refreshToken &&
                a.RefreshTokenExpiry.HasValue &&
                a.RefreshTokenExpiry > DateTime.UtcNow);

            if (admin == null)
                throw new UnauthorizedException("Invalid refresh token");

            var newToken = GenerateToken(admin);
            var newRefreshToken = GenerateRefreshToken();

            admin.RefreshToken = newRefreshToken;
            admin.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            _adminRepository.Update(admin);
            await _unitOfWork.SaveChangesAsync();

            return new AdminAuthResultDto
            {
                Id = admin.Id.ToString(),
                Email = admin.Email,
                FullName = $"{admin.FirstName} {admin.LastName}",
                Token = newToken,
                RefreshToken = newRefreshToken,
                Role = admin.Role
            };
        }

        public async Task<AdminResultDto> GetAdminByIdAsync(Guid id)
        {
            var admin = await _adminRepository.GetByIdAsync(id);
            if (admin == null)
                throw new AdminNotFoundException(id);

            return new AdminResultDto
            {
                Id = admin.Id,
                Email = admin.Email,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Role = admin.Role,
                IsActive = admin.IsActive,
                LastLogin = admin.LastLogin,
                CreatedAt = admin.CreatedAt
            };
        }

        public async Task<IEnumerable<AdminResultDto>> GetAllAdminsAsync()
        {
            var admins = await _adminRepository.GetAllAsync(true);
            return admins.Select(a => new AdminResultDto
            {
                Id = a.Id,
                Email = a.Email,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Role = a.Role,
                IsActive = a.IsActive,
                LastLogin = a.LastLogin,
                CreatedAt = a.CreatedAt
            });
        }

        public async Task<bool> LogoutAsync(Guid adminId)
        {
            var admin = await _adminRepository.GetByIdAsync(adminId);
            if (admin == null)
                return false;

            admin.RefreshToken = null;
            admin.RefreshTokenExpiry = null;
            _adminRepository.Update(admin);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _adminRepository.GetByEmailAsync(email) != null;
        }

        private string GenerateToken(Admin admin)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, admin.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, admin.Email),
                new Claim("admin_id", admin.Id.ToString()),
                new Claim("full_name", $"{admin.FirstName} {admin.LastName}"),
                new Claim(ClaimTypes.Role, admin.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtOptions.ExpirationInDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GetCurrentAdminRole()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }
    }
}