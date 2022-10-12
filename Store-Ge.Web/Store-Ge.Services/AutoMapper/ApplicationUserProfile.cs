using AutoMapper;

using Store_Ge.Data.Models;
using Store_Ge.Services.Models;

namespace Store_Ge.Services.AutoMapper
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile() 
        {
            CreateMap<ApplicationUserRegisterDto, ApplicationUser>();
            CreateMap<ApplicationUserLoginDto, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ReverseMap();
            CreateMap<ApplicationUserLoginResponseDto, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserTokensDto>();
        }
    }
}
