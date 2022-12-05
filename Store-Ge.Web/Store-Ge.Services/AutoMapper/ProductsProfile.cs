using AutoMapper;
using Store_Ge.Data.Models;
using Store_Ge.Services.Models.ProductModels;

namespace Store_Ge.Services.AutoMapper
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<AddProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => int.Parse(src.Id)));
        }
    }
}
