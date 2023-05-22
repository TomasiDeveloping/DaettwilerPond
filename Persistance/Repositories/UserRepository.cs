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
        {
            return new ChangePasswordResponseDto()
            {
                IsSuccessful = false,
                ErrorMessage = "Passwort konnte nicht geändert werden"
            };
        }

        var checkPassword = await _userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword);
        if (!checkPassword)
        {
            return new ChangePasswordResponseDto()
            {
                IsSuccessful = false,
                ErrorMessage = "Aktuelles Passwort ist nicht korrekt"
            };
        }

        var checkChangePassword = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.Password);
        if (checkChangePassword.Succeeded)
        {
            return new ChangePasswordResponseDto()
            {
                IsSuccessful = true,
                ErrorMessage = null
            };
        }

        return new ChangePasswordResponseDto()
        {
            IsSuccessful = false,
            ErrorMessage = "Passwort konnte nicht geändert werden"
        };
    }
}