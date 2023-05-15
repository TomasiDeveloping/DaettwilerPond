using Application.DataTransferObjects.Lsn50V2Measurement;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class Lsn50V2MeasurementProfile : Profile
{
    public Lsn50V2MeasurementProfile()
    {
        CreateMap<Lsn50V2Measurement, Lsn50V2MeasurementDto>().ReverseMap();

        CreateMap<CreateLsn50V2MeasurementDto, Lsn50V2Measurement>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(des => des.ReceivedAt, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<Lsn50V2Measurement, Lsn50V2TemperatureMeasurementDto>();
    }
}