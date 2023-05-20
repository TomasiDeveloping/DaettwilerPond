using Application.DataTransferObjects.Authentication;

namespace Application.Interfaces;

public interface IAuthenticationRepository
{
    Task<RegistrationResponseDto> Register(RegistrationDto registrationDto, string role);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<ForgotPasswordResponseDto> SendForgotPasswordEmailAsync(ForgotPasswordDto forgotPasswordDto);
    Task<ResetPasswordResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
}