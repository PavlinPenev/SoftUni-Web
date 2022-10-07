using Microsoft.AspNetCore.Identity;
using Store_Ge.Services.Models;

namespace Store_Ge.Services.Services.AccountsService
{
    public interface IAccountsService
    {
        Task<ApplicationUserLoginResponseDto> AuthenticateUser(ApplicationUserLoginDto user);
        Task<IdentityResult> RegisterUser(ApplicationUserRegisterDto user);
    }
}
