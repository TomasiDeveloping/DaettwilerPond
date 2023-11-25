using Application.DataTransferObjects.Authentication;
using Application.Interfaces;
using Asp.Versioning;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class AuthenticationsController(IAuthenticationRepository authenticationRepository,
    ILogger<AuthenticationsController> logger) : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        try
        {
            var loginResponse = await authenticationRepository.LoginAsync(loginDto);
            if (loginResponse.IsSuccessful) return Ok(loginResponse);

            return Unauthorized(loginResponse.ErrorMessage);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(Login)}");
        }
    }

    [Authorize(Roles = RoleConstants.Administrator)]
    [HttpPost("[action]")]
    public async Task<ActionResult<RegistrationResponseDto>> Register(RegistrationDto registrationDto)
    {
        try
        {
            var registerResponse = await authenticationRepository.Register(registrationDto);
            if (registerResponse.IsSuccessful) return Ok(registerResponse);
            return BadRequest(registerResponse);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(Register)}");
        }
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<ForgotPasswordResponseDto>> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            var response = await authenticationRepository.SendForgotPasswordEmailAsync(forgotPasswordDto);
            return Ok(response);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(ForgotPassword)}");
        }
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<ResetPasswordResponseDto>> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        try
        {
            var resetPasswordResponse = await authenticationRepository.ResetPasswordAsync(resetPasswordDto);
            if (resetPasswordResponse.IsSuccessful) return Ok(resetPasswordResponse);

            return BadRequest(resetPasswordResponse);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(ResetPassword)}");
        }
    }
}