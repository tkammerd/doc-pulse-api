using AutoMapper;

namespace Doc.Pulse.Api.Setup.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Sample Mapping
            //CreateMap<FromClass, ToClass>().ForMember(
            //    dest => dest.Property,
            //    opt => opt.MapFrom(src => src.PropertyExpression)
            //);
        }
    }
}