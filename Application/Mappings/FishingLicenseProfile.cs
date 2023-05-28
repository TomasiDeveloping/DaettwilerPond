using Application.DataTransferObjects.FishingLicense;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class FishingLicenseProfile : Profile
{
    public FishingLicenseProfile()
    {
        CreateMap<FishingLicense, FishingLicenseDto>()
            .ForMember(des => des.UserFullName,
                opt => opt.MapFrom(scr => $"{scr.User.FirstName} {scr.User.LastName}"));
        CreateMap<FishingLicense, FishingLicenseDto>();

        CreateMap<CreateFishingLicenseDto, FishingLicense>()
            .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(des => des.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));
    }
}