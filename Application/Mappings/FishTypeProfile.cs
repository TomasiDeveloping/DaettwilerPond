using Application.DataTransferObjects.FishType;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

// Mapping profile for FishType entity and its related DTOs using AutoMapper
public class FishTypeProfile : Profile
{
    // Constructor for the FishTypeProfile class
    public FishTypeProfile()
    {
        // Bidirectional mapping from FishType entity to FishTypeDto and vice versa
        CreateMap<FishType, FishTypeDto>().ReverseMap();

        // Mapping from CreateFishTypeDto to FishType entity
        CreateMap<CreateFishTypeDto, FishType>()
            // Custom mapping for Id, generating a new Guid for the destination
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}