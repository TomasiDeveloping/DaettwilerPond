using Application.DataTransferObjects.CatchDetail;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class CatchDetailProfile : Profile
{
    public CatchDetailProfile()
    {
        CreateMap<CatchDetail, CatchDetailDto>()
            .ForMember(des => des.FishTypeName, opt => opt.MapFrom(src => src.FishType.Name));

        CreateMap<CreateCatchDetailDto, CatchDetail>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => new Guid()));

        CreateMap<UpdateCatchDetailDto, CatchDetail>();
    }
}