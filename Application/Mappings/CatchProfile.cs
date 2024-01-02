using Application.DataTransferObjects.Catch;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class CatchProfile : Profile
{
    public CatchProfile()
    {
        CreateMap<Catch, CatchDto>();

        CreateMap<CreateCatchDto, Catch>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => new Guid()));

        CreateMap<UpdateCatchDto, Catch>();
    }
}