using System.IdentityModel.Tokens.Jwt;
using Application.DataTransferObjects.Authentication;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public AuthenticationRepository(UserManager<User> userManager, IJwtService jwtService, IMapper mapper,
        IEmailService emailService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    public async Task<RegistrationResponseDto> Register(RegistrationDto registrationDto, string role)
    {
        var user = _mapper.Map<User>(registrationDto);
        var result = await _userManager.CreateAsync(user, registrationDto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return new RegistrationResponseDto
            {
                IsSuccessful = false,
                Errors = errors
            };
        }

        var roleResult = await _userManager.AddToRoleAsync(user, role);
        if (roleResult.Succeeded)
            return new RegistrationResponseDto
            {
                IsSuccessful = true
            };
        {
            var errors = result.Errors.Select(e => e.Description);
            return new RegistrationResponseDto
            {
                IsSuccessful = false,
                Errors = errors
            };
        }
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            return new AuthResponseDto
            {
                ErrorMessage = "Ungültige Authentifizierung",
                IsSuccessful = false
            };
        if (!user.IsActive)
            return new AuthResponseDto
            {
                ErrorMessage = "Dein Account ist inaktiv",
                IsSuccessful = false
            };
        var signinCredentials = _jwtService.GetSigningCredentials();
        var claims = await _jwtService.GetClaimsAsync(user);
        var tokenOptions = _jwtService.GenerateTokenOptions(signinCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return new AuthResponseDto
        {
            IsSuccessful = true,
            Token = token
        };
    }

    public Task<ForgotPasswordResponseDto> SendForgotPasswordEmailAsync(ForgotPasswordDto forgotPasswordDto)
    {
        throw new NotImplementedException();
    }

    public Task<ResetPasswordResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        throw new NotImplementedException();
    }
}