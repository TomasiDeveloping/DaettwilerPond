using Application.DataTransferObjects.Lsn50V2Measurement;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

// Mapping profile for Lsn50V2Measurement entity and its related DTOs using AutoMapper
public class Lsn50V2MeasurementProfile : Profile
{
    // Constructor for the Lsn50V2MeasurementProfile class
    public Lsn50V2MeasurementProfile()
    {
        // Bidirectional mapping from Lsn50V2Measurement entity to Lsn50V2MeasurementDto and vice versa
        CreateMap<Lsn50V2Measurement, Lsn50V2MeasurementDto>().ReverseMap();

        // Mapping from CreateLsn50V2MeasurementDto to Lsn50V2Measurement entity
        CreateMap<CreateLsn50V2MeasurementDto, Lsn50V2Measurement>()
            // Custom mapping for Id, generating a new Guid for the destination
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            // Custom mapping for ReceivedAt, setting it to the current DateTime
            .ForMember(des => des.ReceivedAt, opt => opt.MapFrom(src => DateTime.Now));

        // Mapping from Lsn50V2Measurement entity to Lsn50V2TemperatureMeasurementDto
        CreateMap<Lsn50V2Measurement, Lsn50V2TemperatureMeasurementDto>();
    }
}