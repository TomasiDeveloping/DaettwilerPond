using Application.DataTransferObjects.User;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();

        CreateMap<User, UserWithAddressDto>()
            .ForMember(des => des.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(des => des.Address,
                opt => opt.MapFrom(src => src.Addresses.FirstOrDefault(u => u.UserId == src.Id)));
        CreateMap<UserWithAddressDto, User>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => src.UserId));
    }
}