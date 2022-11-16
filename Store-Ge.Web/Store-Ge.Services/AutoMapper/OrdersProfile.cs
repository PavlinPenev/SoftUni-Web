using AutoMapper;
using Store_Ge.Data.Models;
using Store_Ge.Services.Models.OrderModels;

namespace Store_Ge.Services.AutoMapper
{
    public class OrdersProfile : Profile
    {
        public OrdersProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<Order, UserOrderDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name));
        }
    }
}
