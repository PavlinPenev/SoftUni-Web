using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Store_Ge.Data.Models;
using Store_Ge.Data.Repositories;
using Store_Ge.Services.Models;
using static Store_Ge.Common.Constants.AccountsConstants;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Store_Ge.Configurations.Services;
using Store_Ge.Services.Configurations;
using Store_Ge.Services.Services.EmailService;

namespace Store_Ge.Services.Services.AccountsService
{
    public class AccountsService : IAccountsService
    {
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IDataProtector dataProtector;
        private readonly StoreGeAppSettings appSettings;
        private readonly JwtSettings jwtSettings;
        private readonly IMapper mapper;

        public AccountsService(
            IRepository<ApplicationUser> usersRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IDataProtectionProvider dataProtectionProvider,
            IOptions<JwtSettings> jwtSettings,
            IOptions<StoreGeAppSettings> appSettings,
            IMapper mapper)
        {
            this.usersRepository = usersRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.appSettings = appSettings.Value;
            this.dataProtector = dataProtectionProvider.CreateProtector(this.appSettings.DataProtectionKey);
            this.jwtSettings = jwtSettings.Value;
            this.mapper = mapper;
        }

        public async Task<ApplicationUserLoginResponseDto> AuthenticateUser(ApplicationUserLoginDto user)
        {
            var findUser = await userManager.FindByEmailAsync(user.Email);

            if (findUser == null)
            {
                throw new NullReferenceException(USER_NOT_FOUND);
            }

            var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(findUser);

            if (!isEmailConfirmed)
            {
                throw new InvalidOperationException(EMAIL_NOT_CONFIRMED);
            }

            var isAuthenticated = await userManager.CheckPasswordAsync(findUser, user.Password);
            if (!isAuthenticated)
            {
                throw new MemberAccessException(WRONG_CREDENTIALS);
            }

            await GenerateTokens(findUser);

            var mappedUser = mapper.Map<ApplicationUserLoginResponseDto>(findUser);
            mappedUser.Id = dataProtector.Protect(mappedUser.Id);

            return mappedUser;
        }

        public async Task<IdentityResult> RegisterUser(ApplicationUserRegisterDto userModel)
        {
            var user = mapper.Map<ApplicationUser>(userModel);

            var rolesInDb = roleManager.Roles.Select(x => x.Name).ToList();
       
            await userManager.CreateAsync(user, userModel.Password);

            var result = await userManager.AddToRolesAsync(user, rolesInDb);

            return result; 
        }

        public async Task<string> GenerateConfirmationEmailToken(ApplicationUser user)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            return token;
        }

        public async Task<ApplicationUserTokensDto> RefreshAccessTokenAsync(string refreshToken, string userId)
        {
            var decodedUserId = dataProtector.Unprotect(userId);
            var user = await userManager.FindByIdAsync(decodedUserId);
            if (user == null)
            {
                throw new NullReferenceException(USER_NOT_FOUND);
            }

            if (refreshToken != user.RefreshToken || DateTime.UtcNow > user.RefreshTokenExpirationDate)
            {
                throw new MemberAccessException(WRONG_CREDENTIALS);
            }

            await GenerateTokens(user);

            var tokensResponse = mapper.Map<ApplicationUserTokensDto>(user);

            return tokensResponse;

        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task<ApplicationUserDto> GetUser(string userId)
        {
            var unprotectedUserId = dataProtector.Unprotect(userId);
            var user = await userManager.FindByIdAsync(unprotectedUserId);

            var mappedUser = mapper.Map<ApplicationUserDto>(user);

            return mappedUser;
        }

        public async Task<IdentityResult> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            var decodedUserId = dataProtector.Unprotect(confirmEmailDto.UserId);

            var user = await userManager.FindByIdAsync(decodedUserId);

            if (user == null)
            {
                throw new NullReferenceException(USER_NOT_FOUND);
            }

            var decodedTokenBytes = WebEncoders.Base64UrlDecode(confirmEmailDto.EmailToken);
            var decodedToken = Encoding.Default.GetString(decodedTokenBytes);

            var result = await userManager.ConfirmEmailAsync(user, decodedToken);
        
            return result;
        }

        public async Task<string> GenerateForgottenPasswordResetToken(ForgotPasswordDto forgotPassword)
        {
            var user = await userManager.FindByEmailAsync(forgotPassword.Email);

            if (user == null)
            {
                throw new NullReferenceException(USER_NOT_FOUND);
            }

            var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            return passwordResetToken;
        }

        public async Task<IdentityResult> PasswordReset(PasswordResetDto passwordResetDto)
        {
            var decodedEmail = dataProtector.Unprotect(passwordResetDto.Email);

            var user = await userManager.FindByEmailAsync(decodedEmail);

            if (user == null)
            {
                throw new NullReferenceException(USER_NOT_FOUND);
            }

            var decodedTokenBytes = WebEncoders.Base64UrlDecode(passwordResetDto.ResetToken);
            var decodedToken = Encoding.Default.GetString(decodedTokenBytes);

            var result = await userManager.ResetPasswordAsync(user, decodedToken, passwordResetDto.Password);

            return result;
        }

        public async Task<IdentityResult> RegisterCashier(AddCashierRequestDto request)
        {
            var user = mapper.Map<ApplicationUser>(request);

            var decryptedStoreId = int.Parse(dataProtector.Unprotect(request.StoreId));

            user.UsersStores.Add(new UserStore
            {
                StoreId = decryptedStoreId,
                User = user
            });

            var cashierRole = await roleManager.FindByIdAsync("2");

            await userManager.CreateAsync(user, request.Password);

            var result = await userManager.AddToRoleAsync(user, cashierRole.Name);

            return result;
        }

        public async Task<bool> UpdateUser(string userId, string email, string userName)
        {
            var decodedUserId = dataProtector.Unprotect(userId);

            var user = await userManager.FindByIdAsync(decodedUserId);

            if (user == null)
            {
                throw new NullReferenceException(USER_NOT_FOUND);
            }

            var emailChangeResult = await userManager.SetEmailAsync(user, email);

            var userNameChangeResult = await userManager.SetUserNameAsync(user, userName);

            return emailChangeResult.Succeeded && userNameChangeResult.Succeeded;
        }

        private async Task GenerateTokens(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                IssuedAt = DateTime.UtcNow
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.AccessToken = tokenHandler.WriteToken(token);
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpirationDate = DateTime.UtcNow.AddHours(1.5);

            usersRepository.Update(user);
            await usersRepository.SaveChangesAsync();
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
