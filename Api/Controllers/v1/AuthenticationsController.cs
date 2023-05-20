using Application.DataTransferObjects.Authentication;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class AuthenticationsController : ControllerBase
{
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly ILogger<AuthenticationsController> _logger;

    public AuthenticationsController(IAuthenticationRepository authenticationRepository,
        ILogger<AuthenticationsController> logger)
    {
        _authenticationRepository = authenticationRepository;
        _logger = logger;
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        try
        {
            var loginResponse = await _authenticationRepository.LoginAsync(loginDto);
            if (loginResponse.IsSuccessful) return Ok(loginResponse);

            return Unauthorized(loginResponse.ErrorMessage);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
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
            var registerResponse = await _authenticationRepository.Register(registrationDto);
            if (registerResponse.IsSuccessful) return Ok(registerResponse);
            return BadRequest(registerResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(Register)}");
        }
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<ForgotPasswordResponseDto>> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            var response = await _authenticationRepository.SendForgotPasswordEmailAsync(forgotPasswordDto);
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(ForgotPassword)}");
        }
    }
}