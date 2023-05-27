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

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        try
        {
            var users = await _userRepository.GetUsersAsync();
            return users.Any() ? Ok(users) : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetUsers)}");
        }
    }

    [HttpGet("Addresses")]
    public async Task<ActionResult<List<UserWithAddressDto>>> GetUsersWithAddresses()
    {
        try
        {
            var usersWithAddresses = await _userRepository.GetUsersWithAddressesAsync();
            return usersWithAddresses.Any() ? Ok(usersWithAddresses) : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetUsersWithAddresses)}");
        }
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

    [HttpPut("Addresses/{userId:guid}")]
    public async Task<ActionResult<UserWithAddressDto>> UpdateUserWithAddress(Guid userId,
        UserWithAddressDto userWithAddressDto)
    {
        try
        {
            if (userId != userWithAddressDto.UserId) return BadRequest("Fehler in Id's");
            var updatedUser = await _userRepository.UpdateUserWithAddressAsync(userWithAddressDto);
            if (updatedUser == null) return BadRequest("User konnte nicht geupdated werden");
            return Ok(updatedUser);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateUserWithAddress)}"); ;
        }
    }

    [HttpPut("{userId:guid}")]
    public async Task<ActionResult<UserDto>> UpdateUser(Guid userId, UserDto userDto)
    {
        try
        {
            if (userId != userDto.Id) return BadRequest("Fehler in Id's");
            var updatedUser = await _userRepository.UpdateUserAsync(userId, userDto);
            if (updatedUser == null) return BadRequest("User konnte nicht geupdated werden");
            return Ok(updatedUser);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateUser)}"); ;
        }
    }

    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult<bool>> DeleteUser(Guid userId)
    {
        try
        {
            var result = await _userRepository.DeleteUserAsync(userId);
            return result ? Ok(true) : BadRequest("User konnte nicht gelöscht werden");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(DeleteUser)}"); ;
        }
    }
}