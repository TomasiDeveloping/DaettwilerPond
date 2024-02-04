using Application.DataTransferObjects.FishingRegulation;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

// Mapping profile for FishingRegulation entity and its related DTOs using AutoMapper
public class FishingRegulationProfile : Profile
{
    // Constructor for the FishingRegulationProfile class
    public FishingRegulationProfile()
    {
        // Bidirectional mapping from FishingRegulation entity to FishingRegulationDto and vice versa
        CreateMap<FishingRegulation, FishingRegulationDto>().ReverseMap();

        // Mapping from CreateFishingRegulationDto to FishingRegulation entity
        CreateMap<CreateFishingRegulationDto, FishingRegulation>()
            // Custom mapping for Id, generating a new Guid for the destination
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}