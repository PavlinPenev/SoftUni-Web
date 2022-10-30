using AutoMapper;
using Store_Ge.Data.Models;
using Store_Ge.Services.Models;

namespace Store_Ge.Services.AutoMapper
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile() 
        {
            CreateMap<ApplicationUser, ApplicationUserDto>();
            CreateMap<ApplicationUserRegisterDto, ApplicationUser>();
            CreateMap<ApplicationUserLoginDto, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserLoginResponseDto>();
            CreateMap<ApplicationUser, ApplicationUserTokensDto>();
        }
    }
}
