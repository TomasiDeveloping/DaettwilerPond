using Application.DataTransferObjects.Catch;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

// Mapping profile for Catch entity and its related DTOs using AutoMapper
public class CatchProfile : Profile
{
    // Constructor for the CatchProfile class
    public CatchProfile()
    {
        // Mapping from Catch entity to CatchDto
        CreateMap<Catch, CatchDto>()
            // Custom mapping for AmountFishCatch, counting the number of CatchDetails
            .ForMember(des => des.AmountFishCatch, opt => opt.MapFrom(scr => scr.CatchDetails.Count));

        // Mapping from CreateCatchDto to Catch entity
        CreateMap<CreateCatchDto, Catch>()
            // Custom mapping for Id, generating a new Guid for the destination
            .ForMember(des => des.Id, opt => opt.MapFrom(src => new Guid()));

        // Mapping from UpdateCatchDto to Catch entity
        CreateMap<UpdateCatchDto, Catch>();
    }
}