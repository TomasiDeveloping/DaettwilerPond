using Application.DataTransferObjects.Address;
using Application.DataTransferObjects.User;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DaettwilerPondDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public UserRepository(DaettwilerPondDbContext context, IMapper mapper, UserManager<User> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _context.Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return users;
    }

    public async Task<List<UserWithAddressDto>> GetUsersWithAddressesAsync()
    {
        var users = await _context.Users.AsNoTracking().OrderBy(u => u.LastName).ToListAsync();
        var usersWithAddresses = new List<UserWithAddressDto>();
        foreach (var user in users.Where(user =>
                     !user.FirstName.Equals("System") || !user.LastName.Equals("Administrator")))
        {
            var role = await _userManager.GetRolesAsync(user);
            var address = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == user.Id);
            usersWithAddresses.Add(new UserWithAddressDto
            {
                UserId = user.Id,
                IsActive = user.IsActive,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = role.FirstOrDefault(),
                Address = _mapper.Map<AddressDto>(address)
            });
        }

        return usersWithAddresses;
    }

    public async Task<UserDto> UpdateUserAsync(Guid userId, UserDto userDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return null;
        _mapper.Map(userDto, user);
        await _context.SaveChangesAsync();
        return await GetUserByIdAsync(userId);
    }

    public async Task<UserWithAddressDto> UpdateUserWithAddressAsync(UserWithAddressDto userWithAddressDto)
    {
        var user = await _context.Users
            .Include(u => u.Addresses)
            .FirstOrDefaultAsync(u => u.Id == userWithAddressDto.UserId);
        if (user == null) return null;
        _mapper.Map(userWithAddressDto, user);
        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.FirstOrDefault() != userWithAddressDto.Role)
        {
            await _userManager.RemoveFromRoleAsync(user, currentRoles.FirstOrDefault() ?? string.Empty);
            await _userManager.AddToRoleAsync(user, userWithAddressDto.Role);
        }

        var address = user.Addresses.FirstOrDefault() ?? new Address();
        address.City = userWithAddressDto.Address.City;
        address.Country = userWithAddressDto.Address.Country;
        address.HouseNumber = userWithAddressDto.Address.HouseNumber;
        address.Mobile = userWithAddressDto.Address.Mobile;
        address.Phone = userWithAddressDto.Address.Phone;
        address.PostalCode = userWithAddressDto.Address.PostalCode;
        address.Street = userWithAddressDto.Address.Street;
        await _context.SaveChangesAsync();
        return userWithAddressDto;
    }

    public async Task<UserDto> GetUserByIdAsync(Guid userId)
    {
        var user = await _context.Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);
        return user;
    }

    public async Task<ChangePasswordResponseDto> ChangeUserPassword(ChangePasswordDto changePasswordDto)
    {
        var user = await _userManager.FindByIdAsync(changePasswordDto.UserId.ToString());
        if (user == null)
            return new ChangePasswordResponseDto
            {
                IsSuccessful = false,
                ErrorMessage = "Passwort konnte nicht geändert werden"
            };

        var checkPassword = await _userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword);
        if (!checkPassword)
            return new ChangePasswordResponseDto
            {
                IsSuccessful = false,
                ErrorMessage = "Aktuelles Passwort ist nicht korrekt"
            };

        var checkChangePassword =
            await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.Password);
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
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return false;
        var addresses = await _context.Addresses.Where(a => a.UserId == userId).ToListAsync();
        if (addresses.Any()) _context.Addresses.RemoveRange(addresses);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}