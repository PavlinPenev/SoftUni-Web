using AutoMapper;
using Store_Ge.Data.Models;
using Store_Ge.Services.Models.AuditTrailModels;

namespace Store_Ge.Services.AutoMapper
{
    public class AuditTrailProfile : Profile
    {
        public AuditTrailProfile()
        {
            CreateMap<AuditEvent, AuditEventDto>();
        }
    }
}
