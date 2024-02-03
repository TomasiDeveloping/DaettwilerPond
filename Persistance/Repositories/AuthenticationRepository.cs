using System.IdentityModel.Tokens.Jwt;
using Application.DataTransferObjects.Authentication;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Persistence.Helpers;

namespace Persistence.Repositories;

public class AuthenticationRepository(UserManager<User> userManager, IJwtService jwtService, IMapper mapper, DaettwilerPondDbContext context,
    IEmailService emailService) : IAuthenticationRepository
{
    public async Task<RegistrationResponseDto> Register(RegistrationDto registrationDto)
    {
        var user = mapper.Map<User>(registrationDto);
        IEnumerable<string> errors;
        var result = await userManager.CreateAsync(user, $"Welcome${DateTime.Now.Year}");
        if (!result.Succeeded)
        {
            errors = result.Errors.Select(e => e.Description);
            return new RegistrationResponseDto
            {
                IsSuccessful = false,
                Errors = errors
            };
        }

        var address = mapper.Map<Address>(registrationDto.Address);
        address.UserId = user.Id;
        await context.Addresses.AddAsync(address);
        await context.SaveChangesAsync();
        var roleResult = await userManager.AddToRoleAsync(user, registrationDto.Role);
        if (roleResult.Succeeded)
            return new RegistrationResponseDto
            {
                IsSuccessful = true
            };
        errors = roleResult.Errors.Select(e => e.Description);
        return new RegistrationResponseDto
        {
            IsSuccessful = false,
            Errors = errors
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
            return new AuthResponseDto
            {
                ErrorMessage = "E-Mail oder Passwort nicht korrekt",
                IsSuccessful = false
            };
        if (!user.IsActive)
            return new AuthResponseDto
            {
                ErrorMessage = "Dein Account ist inaktiv",
                IsSuccessful = false
            };
        var signinCredentials = jwtService.GetSigningCredentials();
        var claims = await jwtService.GetClaimsAsync(user);
        var tokenOptions = jwtService.GenerateTokenOptions(signinCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        user.LastActivity = DateTime.Now;
        await userManager.UpdateAsync(user);
        return new AuthResponseDto
        {
            IsSuccessful = true,
            Token = token
        };
    }

    public async Task<ForgotPasswordResponseDto> SendForgotPasswordEmailAsync(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);
        if (user == null)
            return new ForgotPasswordResponseDto
            {
                IsSuccessful = false,
                ErrorMessage = "Invalid Request"
            };
        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var callback = new Uri(forgotPasswordDto.ClientUri)
            .AddQuery("token", token)
            .AddQuery("email", forgotPasswordDto.Email);
        var message = new EmailMessage(new[] {user.Email}, "Passwort zurücksetzen",
            PasswordResetMessage(user, callback.ToString()));

        await emailService.SendEmailAsync(message);
        return new ForgotPasswordResponseDto
        {
            IsSuccessful = true
        };
    }

    public async Task<ResetPasswordResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user == null)
            return new ResetPasswordResponseDto
            {
                IsSuccessful = false,
                Errors = new[] {"Invalid Request"}
            };

        var resetPasswordResult =
            await userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
        if (resetPasswordResult.Succeeded)
            return new ResetPasswordResponseDto
            {
                IsSuccessful = true
            };

        var errors = resetPasswordResult.Errors.Select(x => x.Description);
        return new ResetPasswordResponseDto
        {
            IsSuccessful = false,
            Errors = errors
        };
    }

    private static string PasswordResetMessage(User user, string callBack)
    {
        return $"<h2>Hallo {user.FirstName} {user.LastName}</h2>" +
               "<p>Du hast kürzlich beantragt, " +
               "das Passwort für das Dättwiler-Weiher Portal zurückzusetzen. " +
               "Klicke auf den Link unten, um fortzufahren.</p>" +
               $"<a href={callBack}>Passwort zurücksetzen</a>" +
               "<p>Wenn Du keine Passwortrücksetzung angefordert hast, " +
               "ignoriere bitte diese E-Mail oder antworte, um uns dies mitzuteilen.</p>" +
               "<p> Dieser Link zum Zurücksetzen des Passworts ist nur für die nächsten <b>2 Stunden gültig</b>.</p>" +
               "<p>Vielen Dank, das Dättwiler-Weiher Portal</p>";
    }

  
}