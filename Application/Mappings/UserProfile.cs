using Application.DataTransferObjects.User;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}