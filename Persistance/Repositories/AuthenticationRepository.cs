using System.IdentityModel.Tokens.Jwt;
using Application.DataTransferObjects.Authentication;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Persistence.Helpers;

namespace Persistence.Repositories;

// AuthenticationRepository handles user registration, login, password reset, etc.
public class AuthenticationRepository(
    UserManager<User> userManager,
    IJwtService jwtService,
    IMapper mapper,
    DaettwilerPondDbContext context,
    IEmailService emailService) : IAuthenticationRepository
{
    // Register a new user
    public async Task<RegistrationResponseDto> Register(RegistrationDto registrationDto)
    {
        // Mapping registrationDto to User entity
        var user = mapper.Map<User>(registrationDto);
        IEnumerable<string> errors;

        // Creating user with a default password
        var result = await userManager.CreateAsync(user, $"Welcome${DateTime.Now.Year}");

        // Handling registration errors and returning the response
        if (!result.Succeeded)
        {
            errors = result.Errors.Select(e => e.Description);
            return new RegistrationResponseDto
            {
                IsSuccessful = false,
                Errors = errors
            };
        }

        // Mapping and adding user's address
        var address = mapper.Map<Address>(registrationDto.Address);
        address.UserId = user.Id;
        await context.Addresses.AddAsync(address);
        await context.SaveChangesAsync();

        // Assigning user role
        var roleResult = await userManager.AddToRoleAsync(user, registrationDto.Role);
        if (roleResult.Succeeded)
            return new RegistrationResponseDto
            {
                IsSuccessful = true
            };

        // Handling role assignment errors and returning the response
        errors = roleResult.Errors.Select(e => e.Description);
        return new RegistrationResponseDto
        {
            IsSuccessful = false,
            Errors = errors
        };
    }

    // User login
    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        // Finding user by email
        var user = await userManager.FindByEmailAsync(loginDto.Email);

        // Checking password, account status, generating JWT token
        if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
            return new AuthResponseDto
            {
                ErrorMessage = "E-Mail oder Passwort nicht korrekt",
                IsSuccessful = false
            };

        // Handling inactive user account
        if (!user.IsActive)
            return new AuthResponseDto
            {
                ErrorMessage = "Dein Account ist inaktiv",
                IsSuccessful = false
            };

        // Generating JWT token
        var signinCredentials = jwtService.GetSigningCredentials();
        var claims = await jwtService.GetClaimsAsync(user);
        var tokenOptions = jwtService.GenerateTokenOptions(signinCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        // Updating last activity and returning the authentication response
        user.LastActivity = DateTime.Now;
        await userManager.UpdateAsync(user);
        return new AuthResponseDto
        {
            IsSuccessful = true,
            Token = token
        };
    }

    // Sending forgot password email
    public async Task<ForgotPasswordResponseDto> SendForgotPasswordEmailAsync(ForgotPasswordDto forgotPasswordDto)
    {
        // Finding user by email
        var user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);

        // Handling invalid user request
        if (user == null)
            return new ForgotPasswordResponseDto
            {
                IsSuccessful = false,
                ErrorMessage = "Invalid Request"
            };

        // Generating reset token and constructing reset link
        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var callback = new Uri(forgotPasswordDto.ClientUri)
            .AddQuery("token", token)
            .AddQuery("email", forgotPasswordDto.Email);

        // Constructing email message and sending it
        var message = new EmailMessage(new[] { user.Email }, "Passwort zurücksetzen",
            PasswordResetMessage(user, callback.ToString()));

        await emailService.SendEmailAsync(message);

        // Returning success response
        return new ForgotPasswordResponseDto
        {
            IsSuccessful = true
        };
    }

    // Resetting user password
    public async Task<ResetPasswordResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        // Finding user by email
        var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);

        // Handling invalid user request
        if (user == null)
            return new ResetPasswordResponseDto
            {
                IsSuccessful = false,
                Errors = new[] { "Invalid Request" }
            };

        // Resetting password and handling errors, returning the response
        var resetPasswordResult =
            await userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
        if (resetPasswordResult.Succeeded)
            return new ResetPasswordResponseDto
            {
                IsSuccessful = true
            };

        // Handling password reset errors and returning the response
        var errors = resetPasswordResult.Errors.Select(x => x.Description);
        return new ResetPasswordResponseDto
        {
            IsSuccessful = false,
            Errors = errors
        };
    }

    // Constructing the email message for password reset
    private static string PasswordResetMessage(User user, string callBack)
    {
        // Constructing HTML email message for password reset
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