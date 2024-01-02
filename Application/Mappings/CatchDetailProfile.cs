using Application.DataTransferObjects.CatchDetail;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class CatchDetailProfile : Profile
{
    public CatchDetailProfile()
    {
        CreateMap<CatchDetail, CatchDetailDto>();

        CreateMap<CreateCatchDetailDto, CatchDetail>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => new Guid()));

        CreateMap<UpdateCatchDetailDto, CatchDetail>();
    }
}