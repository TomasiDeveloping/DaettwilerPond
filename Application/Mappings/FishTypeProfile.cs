using Application.DataTransferObjects.FishType;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class FishTypeProfile : Profile
{
    public FishTypeProfile()
    {
        CreateMap<FishType, FishTypeDto>().ReverseMap();
    }
}