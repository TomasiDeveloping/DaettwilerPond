using Application.DataTransferObjects.Lsn50V2Lifecycle;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

// Mapping profile for Lsn50V2Lifecycle entity and its related DTOs using AutoMapper
public class Lsn50V2LifecycleProfile : Profile
{
    // Constructor for the Lsn50V2LifecycleProfile class
    public Lsn50V2LifecycleProfile()
    {
        // Bidirectional mapping from Lsn50V2Lifecycle entity to Lsn50V2LifecycleDto and vice versa
        CreateMap<Lsn50V2Lifecycle, Lsn50V2LifecycleDto>().ReverseMap();

        // Mapping from CreateLsn50V2LifecycleDto to Lsn50V2Lifecycle entity
        CreateMap<CreateLsn50V2LifecycleDto, Lsn50V2Lifecycle>()
            // Custom mapping for Id, generating a new Guid for the destination
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            // Custom mapping for ReceivedAt, setting it to the current DateTime
            .ForMember(des => des.ReceivedAt, opt => opt.MapFrom(src => DateTime.Now));
    }
}