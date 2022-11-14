using AutoMapper;
using Store_Ge.Data.Models;
using Store_Ge.Services.Models.SupplierModels;

namespace Store_Ge.Services.AutoMapper
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<Supplier, AddOrderSupplierDto>();
        }
    }
}
