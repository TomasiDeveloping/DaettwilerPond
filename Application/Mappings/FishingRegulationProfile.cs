using Application.DataTransferObjects.FishingRegulation;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class FishingRegulationProfile : Profile
{
    public FishingRegulationProfile()
    {
        CreateMap<FishingRegulation, FishingRegulationDto>().ReverseMap();

        CreateMap<CreateFishingRegulationDto, FishingRegulation>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}