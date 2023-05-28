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
                opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));
        CreateMap<FishingLicenseDto, FishingLicense>();

        CreateMap<CreateFishingLicenseDto, FishingLicense>()
            .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(des => des.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));
    }
}