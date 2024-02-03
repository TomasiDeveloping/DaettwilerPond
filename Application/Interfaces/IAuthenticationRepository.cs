using Application.DataTransferObjects.Authentication;

namespace Application.Interfaces;

// Interface for authentication-related operations
public interface IAuthenticationRepository
{
    // Register a new user and return a registration response
    Task<RegistrationResponseDto> Register(RegistrationDto registrationDto);

    // Perform user login and return an authentication response
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

    // Send a forgot password email and return a response
    Task<ForgotPasswordResponseDto> SendForgotPasswordEmailAsync(ForgotPasswordDto forgotPasswordDto);

    // Reset user password and return a response
    Task<ResetPasswordResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
}