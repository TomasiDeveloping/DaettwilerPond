using Application.DataTransferObjects.FishType;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class FishTypeProfile : Profile
{
    public FishTypeProfile()
    {
        CreateMap<FishType, FishTypeDto>().ReverseMap();

        CreateMap<CreateFishTypeDto, FishType>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}