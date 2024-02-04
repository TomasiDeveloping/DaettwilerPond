using Application.DataTransferObjects.Address;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

// Mapping profile for AddressDto and Address entities using AutoMapper
public class AddressProfile : Profile
{
    // Constructor for the AddressProfile class
    public AddressProfile()
    {
        // Creating a mapping from AddressDto to Address and enabling reverse mapping
        CreateMap<AddressDto, Address>().ReverseMap();
    }
}