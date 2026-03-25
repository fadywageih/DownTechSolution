namespace Services
{
    public class AuthenticationService(UserManager<User> _userManager, IMapper _mapper
        , IOptions<JwtOptions> options,  IUnitOfWork _unitOfWork, IEmailService _emailService) : IAuthenticationService
    {
        public async Task<bool> CheckIfEmailExist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }
        public async Task<UserResultDto> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException(email);
            return new UserResultDto(DisplayName: user.FirstName, Email: user.Email, Token: await CreateTokenAsync(user), null);
        }
        public async Task<UserResultDto> Register(RegisterDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
            {
                throw new ValidationException(new[] { "Email is already registered." });
            }
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.Name,
                PhoneNumber = dto.Phone,
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ValidationException(errors);
            }
            await _unitOfWork.SaveChangesAsync();

            return new UserResultDto(DisplayName: user.FirstName, Email: user.Email, Token: await CreateTokenAsync(user));
        }
        public async Task<UserResultDto> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) throw new UnauthorizedException();

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) throw new UnauthorizedException();

            return new UserResultDto(DisplayName: user.FirstName, Email: user.Email, Token: await CreateTokenAsync(user));
        }
        private async Task<string> CreateTokenAsync(User user)
        {
            var JwtOptions = options.Value;
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.FirstName),
        new Claim(ClaimTypes.Email, user.Email),
    };
          
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: JwtOptions.Issuer,
                audience: JwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(JwtOptions.ExpirationInDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<bool> SendResetPasswordEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return true;
            }
            try
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var resetLink = $"http://localhost:4200/auth/reset-password?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";

                var emailMessage = new EmailDto
                {
                    To = user.Email,
                    Subject = "Reset your Password",
                    Body = $@"
                <h1>Reset Your Password</h1>
                <p>Please reset your password by clicking the link below:</p>
                <a href='{resetLink}' style='display: inline-block; padding: 10px 20px; background-color: #4F46E5; color: white; text-decoration: none; border-radius: 5px;'>Reset Password</a>
                <p>Or copy and paste this link into your browser:</p>
                <p>{resetLink}</p>
                <br/>
                <p>If you did not request this, please ignore this email.</p>
                <p>Best regards,<br/>TechHub Team</p>
            "
                };
                await _emailService.SendEmailAsync(emailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> ResetPassword(string email, string token, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            return result.Succeeded;
        }
    }
}
