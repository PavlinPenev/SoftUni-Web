using AutoMapper;
using Store_Ge.Data.Models;
using Store_Ge.Services.Models.StoreModels;

namespace Store_Ge.Services.AutoMapper
{
    public class StoreProfile : Profile
    {
        public StoreProfile()
        {
            CreateMap<Store, StoreDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<AddStoreDto, Store>();
        }
    }
}
