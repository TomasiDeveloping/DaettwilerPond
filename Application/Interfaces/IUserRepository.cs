using Application.DataTransferObjects.User;

namespace Application.Interfaces;

// Interface for managing user-related operations
public interface IUserRepository
{
    // Get a list of all users
    Task<List<UserDto>> GetUsersAsync();

    // Get a user by their email address
    Task<UserDto> GetUserByEmail(string userEmail);

    // Get a list of users with their addresses
    Task<List<UserWithAddressDto>> GetUsersWithAddressesAsync();

    // Get a user with address by their user ID
    Task<UserWithAddressDto> GetUserWithAddressByUserId(Guid userId);

    // Update an existing user by their user ID
    Task<UserDto> UpdateUserAsync(Guid userId, UserDto userDto);

    // Update a user with address information
    Task<UserWithAddressDto> UpdateUserWithAddressAsync(UserWithAddressDto userWithAddressDto);

    // Get a user by their user ID
    Task<UserDto> GetUserByIdAsync(Guid userId);

    // Change the password for a user
    Task<ChangePasswordResponseDto> ChangeUserPassword(ChangePasswordDto changePasswordDto);

    // Delete a user by their user ID
    Task<bool> DeleteUserAsync(Guid userId);
}