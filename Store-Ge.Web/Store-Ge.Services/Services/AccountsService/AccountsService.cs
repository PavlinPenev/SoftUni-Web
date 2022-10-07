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

namespace Store_Ge.Services.Services.AccountsService
{
    public class AccountsService : IAccountsService
    {
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDataProtector dataProtector;
        private readonly JwtSettings jwtSettings;
        private readonly IMapper mapper;

        public AccountsService(
            IRepository<ApplicationUser> usersRepository,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            IOptions<JwtSettings> jwtSettings,
            IMapper mapper)
        {
            this.usersRepository = usersRepository;
            this.userManager = userManager;
            this.dataProtector = dataProtectionProvider.CreateProtector(ACCOUNTS_SERVICE_ACCESS_TOKEN_PURPOSE);
            this.jwtSettings = jwtSettings.Value;
            this.mapper = mapper;
        }

        public async Task<ApplicationUserLoginResponseDto> AuthenticateUser(ApplicationUserLoginDto user)
        {
            var findUser = await userManager.FindByEmailAsync(user.Email);

            if (findUser == null || !findUser.EmailConfirmed)
            {
                return null;
            }

            var isAuthenticated = await userManager.CheckPasswordAsync(findUser, user.Password);
            if (!isAuthenticated)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, findUser.UserName),
                new Claim(ClaimTypes.Email, findUser.Email)
            };

            var userRoles = await userManager.GetRolesAsync(findUser);

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
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            findUser.AccessToken = dataProtector.Protect(tokenHandler.WriteToken(token));
            findUser.RefreshToken = dataProtector.Protect(GenerateRefreshToken());
            findUser.RefreshTokenExpirationDate = DateTime.UtcNow.AddHours(1.5);

            usersRepository.Update(findUser);
            await usersRepository.SaveChangesAsync();

            var mappedUser = mapper.Map<ApplicationUserLoginResponseDto>(findUser);

            return mappedUser;
        }

        public async Task<IdentityResult> RegisterUser(ApplicationUserRegisterDto userModel)
        {
            var user = mapper.Map<ApplicationUser>(userModel);

            var result = await userManager.CreateAsync(user, userModel.Password);
            
            return result; 
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
