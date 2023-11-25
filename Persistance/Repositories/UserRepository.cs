using Application.DataTransferObjects.Address;
using Application.DataTransferObjects.User;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class UserRepository(DaettwilerPondDbContext context, IMapper mapper, UserManager<User> userManager) : IUserRepository
{
    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await context.Users
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return users;
    }

    public async Task<UserDto> GetUserByEmail(string userEmail)
    {
        var user = await context.Users
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == userEmail);
        return user;
    }

    public async Task<List<UserWithAddressDto>> GetUsersWithAddressesAsync()
    {
        var users = await context.Users.AsNoTracking().OrderBy(u => u.LastName).ToListAsync();
        var usersWithAddresses = new List<UserWithAddressDto>();
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
                Address = mapper.Map<AddressDto>(address)
            });
        }

        return usersWithAddresses;
    }

    public async Task<UserWithAddressDto> GetUserWithAddressByUserId(Guid userId)
    {
        var userWithAddress = await context.Users
            .ProjectTo<UserWithAddressDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(u => u.UserId == userId);
        return userWithAddress;
    }

    public async Task<UserDto> UpdateUserAsync(Guid userId, UserDto userDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return null;
        mapper.Map(userDto, user);
        await context.SaveChangesAsync();
        return await GetUserByIdAsync(userId);
    }

    public async Task<UserWithAddressDto> UpdateUserWithAddressAsync(UserWithAddressDto userWithAddressDto)
    {
        var user = await context.Users
            .Include(u => u.Addresses)
            .FirstOrDefaultAsync(u => u.Id == userWithAddressDto.UserId);
        if (user == null) return null;
        mapper.Map(userWithAddressDto, user);
        var currentRoles = await userManager.GetRolesAsync(user);
        if (currentRoles.FirstOrDefault() != userWithAddressDto.Role)
        {
            await userManager.RemoveFromRoleAsync(user, currentRoles.FirstOrDefault() ?? string.Empty);
            await userManager.AddToRoleAsync(user, userWithAddressDto.Role);
        }

        var address = user.Addresses.FirstOrDefault() ?? new Address();
        address.City = userWithAddressDto.Address.City;
        address.Country = userWithAddressDto.Address.Country;
        address.HouseNumber = userWithAddressDto.Address.HouseNumber;
        address.Mobile = userWithAddressDto.Address.Mobile;
        address.Phone = userWithAddressDto.Address.Phone;
        address.PostalCode = userWithAddressDto.Address.PostalCode;
        address.Street = userWithAddressDto.Address.Street;
        await context.SaveChangesAsync();
        return userWithAddressDto;
    }

    public async Task<UserDto> GetUserByIdAsync(Guid userId)
    {
        var user = await context.Users
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);
        return user;
    }

    public async Task<ChangePasswordResponseDto> ChangeUserPassword(ChangePasswordDto changePasswordDto)
    {
        var user = await userManager.FindByIdAsync(changePasswordDto.UserId.ToString());
        if (user == null)
            return new ChangePasswordResponseDto
            {
                IsSuccessful = false,
                ErrorMessage = "Passwort konnte nicht geändert werden"
            };

        var checkPassword = await userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword);
        if (!checkPassword)
            return new ChangePasswordResponseDto
            {
                IsSuccessful = false,
                ErrorMessage = "Aktuelles Passwort ist nicht korrekt"
            };

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

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return false;
        var addresses = await context.Addresses.Where(a => a.UserId == userId).ToListAsync();
        if (addresses.Any()) context.Addresses.RemoveRange(addresses);
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return true;
    }
}