using Application.DataTransferObjects.Address;

namespace Application.Interfaces;

// Interface for managing address-related operations
public interface IAddressRepository
{
    // Get a list of all addresses
    Task<List<AddressDto>> GetAddressesAsync();

    // Get a list of addresses associated with a specific user
    Task<List<AddressDto>> GetUserAddressesAsync(Guid userId);

    // Get a specific address by its ID
    Task<AddressDto> GetAddressAsync(Guid addressId);

    // Create a new address
    Task<AddressDto> CreateAddressAsync(AddressDto addressDto);

    // Update an existing address by its ID
    Task<AddressDto> UpdateAddressAsync(Guid addressId, AddressDto addressDto);

    // Delete an address by its ID
    Task<bool> DeleteAddressAsync(Guid addressId);
}