using Application.DataTransferObjects.Authentication;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

// Mapping profile for RegistrationDto and User entities using AutoMapper
public class AuthenticationProfile : Profile
{
    // Constructor for the AuthenticationProfile class
    public AuthenticationProfile()
    {
        // Creating a mapping from RegistrationDto to User
        CreateMap<RegistrationDto, User>()
            // Custom mapping for UserName, using Email property from source
            .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email))
            // Custom mapping for Id, generating a new Guid for the destination
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}