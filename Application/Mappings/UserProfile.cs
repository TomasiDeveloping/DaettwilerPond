using Application.DataTransferObjects.User;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

// Mapping profile for User entity and its related DTOs using AutoMapper
public class UserProfile : Profile
{
    // Constructor for the UserProfile class
    public UserProfile()
    {
        // Bidirectional mapping from User entity to UserDto and vice versa
        CreateMap<User, UserDto>().ReverseMap();

        // Mapping from User entity to UserWithAddressDto
        CreateMap<User, UserWithAddressDto>()
            // Custom mapping for UserId, mapping it from Id property of source
            .ForMember(des => des.UserId, opt => opt.MapFrom(src => src.Id))
            // Custom mapping for Address, using the first matching Address for the user
            .ForMember(des => des.Address,
                opt => opt.MapFrom(src => src.Addresses.FirstOrDefault(u => u.UserId == src.Id)));

        // Reverse mapping from UserWithAddressDto to User entity
        CreateMap<UserWithAddressDto, User>()
            // Custom mapping for Id, mapping it from UserId property of source
            .ForMember(des => des.Id, opt => opt.MapFrom(src => src.UserId));
    }
}