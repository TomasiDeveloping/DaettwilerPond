using Application.DataTransferObjects.Address;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<AddressDto, Address>().ReverseMap();
    }
}