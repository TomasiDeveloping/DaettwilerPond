using Application.DataTransferObjects.FishingClub;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

// Mapping profile for FishingClub entity and its related DTOs using AutoMapper
public class FishingClubProfile : Profile
{
    // Constructor for the FishingClubProfile class
    public FishingClubProfile()
    {
        // Bidirectional mapping from FishingClub entity to FishingClubDto and vice versa
        CreateMap<FishingClub, FishingClubDto>().ReverseMap();

        // Mapping from CreateFishingClubDto to FishingClub entity
        CreateMap<CreateFishingClubDto, FishingClub>()
            // Custom mapping for Id, generating a new Guid for the destination
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}