using Application.DataTransferObjects.SensorType;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class SensorTypeProfile : Profile
{
    public SensorTypeProfile()
    {
        CreateMap<SensorType, SensorTypeDto>().ReverseMap();
    }
}