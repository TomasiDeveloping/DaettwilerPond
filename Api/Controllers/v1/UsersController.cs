using Application.DataTransferObjects.User;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserDto>> GetUserById(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return NotFound($"No user found with id: {userId}");
            return Ok(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetUserById)}");
        }
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<ChangePasswordResponseDto>> ChangeUserPassword(ChangePasswordDto changePasswordDto)
    {
        try
        {
            var changePasswordResponse = await _userRepository.ChangeUserPassword(changePasswordDto);
            if (changePasswordResponse.IsSuccessful) return Ok(changePasswordResponse);
            return BadRequest(changePasswordResponse.ErrorMessage);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(ChangeUserPassword)}");
        }
    }
}