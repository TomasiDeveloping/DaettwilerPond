using Application.Models.CatchReport;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class CatchReportProfile : Profile
{
    public CatchReportProfile()
    {
        CreateMap<FishingLicense, UserStatistic>()
            .ForMember(des => des.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(des => des.LastName, opt => opt.MapFrom(scr => scr.User.LastName));

        CreateMap<Catch, UserCatch>()
            .ForMember(des => des.CatchDate, opt => opt.MapFrom(src => src.CatchDate))
            .ForMember(des => des.HoursSpend, opt => opt.MapFrom(src => src.HoursSpent));

        CreateMap<CatchDetail, UserCatchDetail>()
            .ForMember(des => des.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(des => des.HadCrabs, opt => opt.MapFrom(src => src.HadCrabs))
            .ForMember(des => des.FishTypeName, opt => opt.MapFrom(src => src.FishType.Name));
    }
}