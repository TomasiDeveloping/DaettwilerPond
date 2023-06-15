using Application.DataTransferObjects.User;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<List<UserDto>> GetUsersAsync();
    Task<UserDto> GetUserByEmail(string userEmail);
    Task<List<UserWithAddressDto>> GetUsersWithAddressesAsync();
    Task<UserWithAddressDto> GetUserWithAddressByUserId(Guid userId);
    Task<UserDto> UpdateUserAsync(Guid userId, UserDto userDto);
    Task<UserWithAddressDto> UpdateUserWithAddressAsync(UserWithAddressDto userWithAddressDto);
    Task<UserDto> GetUserByIdAsync(Guid userId);
    Task<ChangePasswordResponseDto> ChangeUserPassword(ChangePasswordDto changePasswordDto);
    Task<bool> DeleteUserAsync(Guid userId);
}