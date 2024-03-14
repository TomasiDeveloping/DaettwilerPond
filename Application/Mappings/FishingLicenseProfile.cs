using Application.DataTransferObjects.FishingLicense;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

// Mapping profile for FishingLicense entity and its related DTOs using AutoMapper
public class FishingLicenseProfile : Profile
{
    // Constructor for the FishingLicenseProfile class
    public FishingLicenseProfile()
    {
        // Mapping from FishingLicense entity to FishingLicenseDto
        CreateMap<FishingLicense, FishingLicenseDto>()
            .ForMember(des => des.UserDateOfBirth, opt => opt.MapFrom(src => src.User.DateOfBirth))
            .ForMember(des => des.UserSanaNumber, opt => opt.MapFrom(src => src.User.SaNaNumber))
            .ForMember(des => des.UserImageUrl, opt => opt.MapFrom(src => src.User.ImageUrl))
            // Custom mapping for UserFullName, combining FirstName and LastName from User
            .ForMember(des => des.UserFullName,
                opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

        // Reverse mapping from FishingLicenseDto to FishingLicense
        CreateMap<FishingLicenseDto, FishingLicense>();

        // Mapping from CreateFishingLicenseDto to FishingLicense entity
        CreateMap<CreateFishingLicenseDto, FishingLicense>()
            // Custom mapping for CreatedAt and UpdatedAt, setting both to current DateTime
            .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(des => des.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));
    }
}