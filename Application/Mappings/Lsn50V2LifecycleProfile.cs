using Application.DataTransferObjects.Lsn50V2Lifecycle;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class Lsn50V2LifecycleProfile : Profile
{
    public Lsn50V2LifecycleProfile()
    {
        CreateMap<Lsn50V2Lifecycle, Lsn50V2LifecycleDto>().ReverseMap();

        CreateMap<CreateLsn50V2LifecycleDto, Lsn50V2Lifecycle>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(des => des.ReceivedAt, opt => opt.MapFrom(src => DateTime.Now));
    }
}