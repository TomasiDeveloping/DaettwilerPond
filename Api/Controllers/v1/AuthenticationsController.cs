using Application.DataTransferObjects.Authentication;
using Application.Interfaces;
using Asp.Versioning;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

// Define the route, API version, and authorize the controller
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiController]
[ApiVersion("1.0")]
public class AuthenticationsController(IAuthenticationRepository authenticationRepository,
    ILogger<AuthenticationsController> logger) : ControllerBase
{
    // Handle POST request to perform user login
    [HttpPost]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        try
        {
            var loginResponse = await authenticationRepository.LoginAsync(loginDto);
            if (loginResponse.IsSuccessful) return Ok(loginResponse);

            // Return Unauthorized status and error message if login fails
            return Unauthorized(loginResponse.ErrorMessage);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(Login)}");
        }
    }

    // Handle POST request to perform user registration (Admin role required)
    [Authorize(Roles = RoleConstants.Administrator)]
    [HttpPost]
    public async Task<ActionResult<RegistrationResponseDto>> Register(RegistrationDto registrationDto)
    {
        try
        {
            var registerResponse = await authenticationRepository.Register(registrationDto);
            if (registerResponse.IsSuccessful) return Ok(registerResponse);

            // Return BadRequest status and error message if registration fails
            return BadRequest(registerResponse);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(Register)}");
        }
    }

    // Handle POST request to initiate the forgot password process
    [HttpPost]
    public async Task<ActionResult<ForgotPasswordResponseDto>> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            var response = await authenticationRepository.SendForgotPasswordEmailAsync(forgotPasswordDto);
            return Ok(response);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(ForgotPassword)}");
        }
    }

    // Handle POST request to reset user password
    [HttpPost]
    public async Task<ActionResult<ResetPasswordResponseDto>> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        try
        {
            var resetPasswordResponse = await authenticationRepository.ResetPasswordAsync(resetPasswordDto);
            if (resetPasswordResponse.IsSuccessful) return Ok(resetPasswordResponse);

            // Return BadRequest status and error message if password reset fails
            return BadRequest(resetPasswordResponse);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(ResetPassword)}");
        }
    }
}