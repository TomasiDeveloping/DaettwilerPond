using Application.DataTransferObjects.Address;
using Application.DataTransferObjects.User;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

// UserRepository handles CRUD operations for User entities, including user addresses and password changes.
public class UserRepository(DaettwilerPondDbContext context, IMapper mapper, UserManager<User> userManager) : IUserRepository
{

    // Get a list of all users
    public async Task<List<UserDto>> GetUsersAsync()
    {
        // Projecting and querying a list of UserDto
        var users = await context.Users
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return users;
    }

    // Get a user by email
    public async Task<UserDto> GetUserByEmail(string userEmail)
    {
        // Projecting and querying a UserDto based on email
        var user = await context.Users
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == userEmail);
        return user;
    }

    // Get a list of users with associated addresses
    public async Task<List<UserWithAddressDto>> GetUsersWithAddressesAsync()
    {
        // Retrieve users with addresses, excluding System Administrators
        var users = await context.Users.AsNoTracking().OrderBy(u => u.LastName).ToListAsync();
        var usersWithAddresses = new List<UserWithAddressDto>();

        // Iterate through users and map to UserWithAddressDto
        foreach (var user in users.Where(user =>
                     !user.FirstName.Equals("System") || !user.LastName.Equals("Administrator")))
        {
            var role = await userManager.GetRolesAsync(user);
            var address = await context.Addresses.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == user.Id);
            usersWithAddresses.Add(new UserWithAddressDto
            {
                UserId = user.Id,
                IsActive = user.IsActive,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = role.FirstOrDefault(),
                SaNaNumber = user.SaNaNumber,
                ImageUrl = user.ImageUrl,
                DateOfBirth = user.DateOfBirth,
                Address = mapper.Map<AddressDto>(address)
            });
        }

        return usersWithAddresses;
    }

    // Get a user with associated address by user ID
    public async Task<UserWithAddressDto> GetUserWithAddressByUserId(Guid userId)
    {
        // Projecting and querying a UserWithAddressDto based on user ID
        var userWithAddress = await context.Users
            .ProjectTo<UserWithAddressDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(u => u.UserId == userId);
        return userWithAddress;
    }

    // Update user details by user ID
    public async Task<UserDto> UpdateUserAsync(Guid userId, UserDto userDto)
    {
        // Retrieve and update user details
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return null;
        if (userDto.Email != user.Email)
        {
            var changeEmailSuccess = await ChangeUserEmail(user, userDto.Email);
            if (!changeEmailSuccess) return null;
            user.UserName = userDto.Email;
            user.NormalizedUserName = userDto.Email.ToUpper();
        }

        userDto.DateOfBirth = userDto.DateOfBirth.AddDays(1).AddSeconds(-1);
        mapper.Map(userDto, user);
        await context.SaveChangesAsync();
        return await GetUserByIdAsync(userId);
    }

    // Update user details with associated address and role
    public async Task<UserWithAddressDto> UpdateUserWithAddressAsync(UserWithAddressDto userWithAddressDto)
    {
        // Retrieve and update user details, address, and role
        var user = await context.Users
            .Include(u => u.Addresses)
            .FirstOrDefaultAsync(u => u.Id == userWithAddressDto.UserId);
        if (user == null) return null;
        if (user.Email != userWithAddressDto.Email)
        {
            var changeEmailSuccess = await ChangeUserEmail(user, userWithAddressDto.Email);
            if (!changeEmailSuccess) return null;
            user.UserName = userWithAddressDto.Email;
            user.NormalizedUserName = userWithAddressDto.Email.ToUpper();
        }

        userWithAddressDto.DateOfBirth = userWithAddressDto.DateOfBirth.AddDays(1).AddSeconds(-1);
        mapper.Map(userWithAddressDto, user);

        // Retrieve and update roles
        var currentRoles = await userManager.GetRolesAsync(user);
        if (currentRoles.FirstOrDefault() != userWithAddressDto.Role)
        {
            await userManager.RemoveFromRoleAsync(user, currentRoles.FirstOrDefault() ?? string.Empty);
            await userManager.AddToRoleAsync(user, userWithAddressDto.Role);
        }

        // Update address details
        var address = user.Addresses.FirstOrDefault() ?? new Address();
        address.City = userWithAddressDto.Address.City;
        address.Country = userWithAddressDto.Address.Country;
        address.HouseNumber = userWithAddressDto.Address.HouseNumber;
        address.Mobile = userWithAddressDto.Address.Mobile;
        address.Phone = userWithAddressDto.Address.Phone;
        address.PostalCode = userWithAddressDto.Address.PostalCode;
        address.Street = userWithAddressDto.Address.Street;

        // Save changes to the database
        await context.SaveChangesAsync();
        return userWithAddressDto;
    }

    // Get a user by user ID
    public async Task<UserDto> GetUserByIdAsync(Guid userId)
    {
        // Projecting and querying a UserDto based on user ID
        var user = await context.Users
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);
        return user;
    }

    // Change user password
    public async Task<ChangePasswordResponseDto> ChangeUserPassword(ChangePasswordDto changePasswordDto)
    {
        // Retrieve user by ID
        var user = await userManager.FindByIdAsync(changePasswordDto.UserId.ToString());
        if (user == null)
            return new ChangePasswordResponseDto
            {
                IsSuccessful = false,
                ErrorMessage = "Passwort konnte nicht geändert werden"
            };

        // Check the current password
        var checkPassword = await userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword);
        if (!checkPassword)
            return new ChangePasswordResponseDto
            {
                IsSuccessful = false,
                ErrorMessage = "Aktuelles Passwort ist nicht korrekt"
            };

        // Change the password
        var checkChangePassword =
            await userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.Password);
        if (checkChangePassword.Succeeded)
            return new ChangePasswordResponseDto
            {
                IsSuccessful = true,
                ErrorMessage = null
            };

        return new ChangePasswordResponseDto
        {
            IsSuccessful = false,
            ErrorMessage = "Passwort konnte nicht geändert werden"
        };
    }

    public async Task<bool> UploadImageAsync(Guid userId, IFormFile file)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return false;

        var folderName = Path.Combine("wwwroot", "Resources", "Images");
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        var fileName = $"{Guid.NewGuid()}.png";
        var fullPath = Path.Combine(pathToSave, fileName);

        if (!string.IsNullOrEmpty(user.ImageUrl))
        {
            var filetToDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ImageUrl);
            if (File.Exists(filetToDelete))
            {
                File.Delete(filetToDelete);
            }
        }

        user.ImageUrl = Path.Combine("Resources", "Images", fileName);
        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        await context.SaveChangesAsync();
        return true;
    }

    // Delete user by user ID
    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        // Retrieve and delete user and associated addresses
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return false;

        // Retrieve and delete associated addresses
        var addresses = await context.Addresses.Where(a => a.UserId == userId).ToListAsync();
        if (addresses.Any()) context.Addresses.RemoveRange(addresses);

        // Remove the user from the context
        context.Users.Remove(user);

        // Save changes to the database
        await context.SaveChangesAsync();

        // Return true to indicate successful deletion
        return true;
    }

    private async Task<bool> ChangeUserEmail(User user, string newEmail)
    {
        var mailToken = await userManager.GenerateChangeEmailTokenAsync(user, newEmail);
        if (string.IsNullOrEmpty(mailToken)) return false;
        var changeEmailResult = await userManager.ChangeEmailAsync(user, newEmail, mailToken);
        return changeEmailResult.Succeeded;
    }
}