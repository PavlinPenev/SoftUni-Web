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

namespace Store_Ge.Services.Services.AccountsService
{
    public class AccountsService : IAccountsService
    {
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IDataProtector dataProtector;
        private readonly JwtSettings jwtSettings;
        private readonly IMapper mapper;

        public AccountsService(
            IRepository<ApplicationUser> usersRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IDataProtectionProvider dataProtectionProvider,
            IOptions<JwtSettings> jwtSettings,
            IMapper mapper)
        {
            this.usersRepository = usersRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dataProtector = dataProtectionProvider.CreateProtector(ACCOUNTS_SERVICE_ACCESS_TOKEN_PURPOSE);
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

            return mappedUser;
        }

        public async Task<IdentityResult> RegisterUser(ApplicationUserRegisterDto userModel)
        {
            var user = mapper.Map<ApplicationUser>(userModel);

            var rolesInDb = roleManager.Roles.Select(x => x.Name).ToList();
       
            var result = await userManager.CreateAsync(user, userModel.Password);

            await userManager.AddToRolesAsync(user, rolesInDb);

            return result; 
        }

        public async Task<string> GenerateConfirmationEmailToken(ApplicationUser user)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            return token;
        }

        public async Task<ApplicationUserTokensDto> RefreshAccessTokenAsync(string refreshToken, int userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new NullReferenceException(USER_NOT_FOUND);
            }

            var userOldRefreshToken = dataProtector.Unprotect(user.RefreshToken);

            if (refreshToken != userOldRefreshToken)
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

        public async Task<IdentityResult> ConfirmEmail(int userId, string emailToken)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            var decodedTokenBytes = WebEncoders.Base64UrlDecode(emailToken);
            var decodedToken = Encoding.Default.GetString(decodedTokenBytes);

            var result = await userManager.ConfirmEmailAsync(user, decodedToken);
        
            return result;
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
            user.AccessToken = dataProtector.Protect(tokenHandler.WriteToken(token));
            user.RefreshToken = dataProtector.Protect(GenerateRefreshToken());
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
