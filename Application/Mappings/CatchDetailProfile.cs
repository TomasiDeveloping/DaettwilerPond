using Application.DataTransferObjects.CatchDetail;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

// Mapping profile for CatchDetail entity and its related DTOs using AutoMapper
public class CatchDetailProfile : Profile
{
    // Constructor for the CatchDetailProfile class
    public CatchDetailProfile()
    {
        // Mapping from CatchDetail entity to CatchDetailDto
        CreateMap<CatchDetail, CatchDetailDto>()
            // Custom mapping for FishTypeName, using FishType.Name property from source
            .ForMember(des => des.FishTypeName, opt => opt.MapFrom(src => src.FishType.Name));

        // Mapping from CreateCatchDetailDto to CatchDetail entity
        CreateMap<CreateCatchDetailDto, CatchDetail>()
            // Custom mapping for Id, generating a new Guid for the destination
            .ForMember(des => des.Id, opt => opt.MapFrom(src => new Guid()))
            .ForMember(des => des.Amount, opt => opt.MapFrom(src => 1));

        // Mapping from UpdateCatchDetailDto to CatchDetail entity
        CreateMap<UpdateCatchDetailDto, CatchDetail>();
    }
}