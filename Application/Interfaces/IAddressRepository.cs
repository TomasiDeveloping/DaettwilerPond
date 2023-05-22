using Application.DataTransferObjects.Address;

namespace Application.Interfaces;

public interface IAddressRepository
{
    Task<List<AddressDto>> GetAddressesAsync();
    Task<List<AddressDto>> GetUserAddressesAsync(Guid userId);
    Task<AddressDto> GetAddressAsync(Guid addressId);
    Task<AddressDto> CreateAddressAsync(AddressDto addressDto);
    Task<AddressDto> UpdateAddressAsync(Guid addressId, AddressDto addressDto);
    Task<bool> DeleteAddressAsync(Guid addressId);
}