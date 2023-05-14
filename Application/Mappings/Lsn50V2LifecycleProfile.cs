using Application.DataTransferObjects.Lsn50V2Lifecycle;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class Lsn50V2LifecycleProfile : Profile
{
    public Lsn50V2LifecycleProfile()
    {
        CreateMap<Lsn50V2Lifecycle, Lsn50V2LifecycleDto>().ReverseMap();
    }
}