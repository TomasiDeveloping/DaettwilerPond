using Application.DataTransferObjects.SensorType;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

// Mapping profile for SensorType entity and its related DTOs using AutoMapper
public class SensorTypeProfile : Profile
{
    // Constructor for the SensorTypeProfile class
    public SensorTypeProfile()
    {
        // Bidirectional mapping from SensorType entity to SensorTypeDto and vice versa
        CreateMap<SensorType, SensorTypeDto>().ReverseMap();

        // Mapping from CreateSensorTypeDto to SensorType entity
        CreateMap<CreateSensorTypeDto, SensorType>()
            // Custom mapping for Id, generating a new Guid for the destination
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}