using Application.DataTransferObjects.Sensor;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

// Mapping profile for Sensor entity and its related DTOs using AutoMapper
public class SensorProfile : Profile
{
    // Constructor for the SensorProfile class
    public SensorProfile()
    {
        // Mapping from Sensor entity to SensorDto
        CreateMap<Sensor, SensorDto>()
            // Custom mapping for SensorTypeName, using Name property from SensorType in source
            .ForMember(des => des.SensorTypeName, opt => opt.MapFrom(src => src.SensorType.Name))
            // Custom mapping for SensorTypeProducer, using Producer property from SensorType in source
            .ForMember(des => des.SensorTypeProducer, opt => opt.MapFrom(src => src.SensorType.Producer));

        // Reverse mapping from SensorDto to Sensor
        CreateMap<SensorDto, Sensor>();

        // Mapping from CreateSensorDto to Sensor entity
        CreateMap<CreateSensorDto, Sensor>()
            // Custom mapping for CreatedAt, setting it to the current DateTime
            .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            // Custom mapping for Id, generating a new Guid for the destination
            .ForMember(des => des.Id, opt => opt.MapFrom(des => Guid.NewGuid()));

        // Mapping from UpdateSensorDto to Sensor entity
        CreateMap<UpdateSensorDto, Sensor>();
    }
}