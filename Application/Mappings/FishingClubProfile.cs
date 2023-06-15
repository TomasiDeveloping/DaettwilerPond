using Application.DataTransferObjects.FishingClub;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class FishingClubProfile : Profile
{
    public FishingClubProfile()
    {
        CreateMap<FishingClub, FishingClubDto>().ReverseMap();

        CreateMap<CreateFishingClubDto, FishingClub>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}