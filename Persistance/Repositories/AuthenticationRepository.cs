using System.IdentityModel.Tokens.Jwt;
using Application.DataTransferObjects.Authentication;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Persistence.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;

    public AuthenticationRepository(UserManager<User> userManager, IJwtService jwtService, IMapper mapper,
        IEmailService emailService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<RegistrationResponseDto> Register(RegistrationDto registrationDto)
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

        var roleResult = await _userManager.AddToRoleAsync(user, registrationDto.Role);
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

    public async Task<ForgotPasswordResponseDto> SendForgotPasswordEmailAsync(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
        if (user == null)
        {
            return new ForgotPasswordResponseDto()
            {
                IsSuccessful = false,
                ErrorMessage = "Invalid Request"
            };
        }
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var param = new Dictionary<string, string>()
        {
            {"token", token},
            {"email", forgotPasswordDto.Email}
        };
        var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientUri, param);
        var message = new EmailMessage(new[] { user.Email }, "Passwort zurücksetzen", PasswordResetMessage(user, callback));

        await _emailService.SendEmailAsync(message);
        return new ForgotPasswordResponseDto
        {
            IsSuccessful = true
        };
    }

    public Task<ResetPasswordResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        throw new NotImplementedException();
    }

    private static string PasswordResetMessage(User user, string callBack)
    {
        return $"<h2>Hallo {user.FirstName} {user.LastName}</h2>" +
               "<p>Sie haben kürzlich beantragt, " +
               "das Passwort für das Dättwiler-Weiher Portal zurückzusetzen." +
               "Klicken Sie auf den Link unten, um fortzufahren.</p>" +
               $"<a href={callBack}>Passwort zurücksetzen</a>" +
               "<p>Wenn Sie keine Passwortrücksetzung angefordert haben, " +
               "ignorieren Sie bitte diese E-Mail oder antworten Sie, um uns dies mitzuteilen.</p>" +
               "<p> Dieser Link zum Zurücksetzen des Passworts ist nur für die nächsten <b>2 Stunden gültig</b>.</p>" +
               "<p>Vielen Dank, das Dättwiler-Weiher Portal</p>";
    }
}