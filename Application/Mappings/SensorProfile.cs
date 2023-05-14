using Application.DataTransferObjects.Sensor;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class SensorProfile : Profile
{
    public SensorProfile()
    {
        CreateMap<Sensor, SensorDto>()
            .ForMember(des => des.SensorTypeName, opt => opt.MapFrom(src => src.SensorType.Name))
            .ForMember(des => des.SensorTypeProducer, opt => opt.MapFrom(src => src.SensorType.Producer));
        CreateMap<SensorDto, Sensor>();
    }
}