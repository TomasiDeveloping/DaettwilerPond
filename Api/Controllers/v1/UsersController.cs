using Application.DataTransferObjects.User;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

// Define the route for the controller with versioning support
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class UsersController(IUserRepository userRepository, ILogger<UsersController> logger) : ControllerBase
{

    // Get a list of users
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        try
        {
            var users = await userRepository.GetUsersAsync();
            return users.Count != 0 ? Ok(users) : NoContent();
        }
        catch (Exception e)
        {
            // Log error and return a 500 Internal Server Error
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetUsers)}");
        }
    }

    // Get a list of users with their addresses
    [HttpGet("Addresses")]
    public async Task<ActionResult<List<UserWithAddressDto>>> GetUsersWithAddresses()
    {
        try
        {
            var usersWithAddresses = await userRepository.GetUsersWithAddressesAsync();
            return usersWithAddresses.Count != 0 ? Ok(usersWithAddresses) : NoContent();
        }
        catch (Exception e)
        {
            // Log error and return a 500 Internal Server Error
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetUsersWithAddresses)}");
        }
    }

    // Get a user by their ID
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserDto>> GetUserById(Guid userId)
    {
        try
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null) return NotFound($"No user found with id: {userId}");
            return Ok(user);
        }
        catch (Exception e)
        {
            // Log error and return a 500 Internal Server Error
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetUserById)}");
        }
    }

    // Change user password
    [HttpPost("[action]")]
    public async Task<ActionResult<ChangePasswordResponseDto>> ChangeUserPassword(ChangePasswordDto changePasswordDto)
    {
        try
        {
            var changePasswordResponse = await userRepository.ChangeUserPassword(changePasswordDto);
            if (changePasswordResponse.IsSuccessful) return Ok(changePasswordResponse);
            return BadRequest(changePasswordResponse.ErrorMessage);
        }
        catch (Exception e)
        {
            // Log error and return a 500 Internal Server Error
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(ChangeUserPassword)}");
        }
    }

    [HttpPost("UploadProfile")]
    public async Task<ActionResult<bool>> UploadImage([FromForm] UploadUserProfileDto uploadUserProfileDto)
    {
        try
        {
            if (!Request.Form.Files.Any())
            {
                return BadRequest("No files found in the request");
            }

            if (Request.Form.Files.Count > 1)
            {
                return BadRequest("Cannot upload more than one file at time");
            }

            var uploadResult =
                await userRepository.UploadImageAsync(uploadUserProfileDto.UserId, uploadUserProfileDto.File);

            return uploadResult ? Ok(true) : BadRequest("Error");
        }
        catch (Exception e)
        {
            // Log error and return a 500 Internal Server Error
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UploadImage)}");
        }
    }

    // Update user with address information
    [HttpPut("Addresses/{userId:guid}")]
    public async Task<ActionResult<UserWithAddressDto>> UpdateUserWithAddress(Guid userId,
        UserWithAddressDto userWithAddressDto)
    {
        try
        {
            if (userId != userWithAddressDto.UserId) return BadRequest("Fehler in Id's");
            var updatedUser = await userRepository.UpdateUserWithAddressAsync(userWithAddressDto);
            if (updatedUser == null) return BadRequest("User konnte nicht geupdated werden");
            return Ok(updatedUser);
        }
        catch (Exception e)
        {
            // Log error and return a 500 Internal Server Error
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateUserWithAddress)}");
        }
    }

    // Update user information
    [HttpPut("{userId:guid}")]
    public async Task<ActionResult<UserDto>> UpdateUser(Guid userId, UserDto userDto)
    {
        try
        {
            if (userId != userDto.Id) return BadRequest("Fehler in Id's");
            var updatedUser = await userRepository.UpdateUserAsync(userId, userDto);
            if (updatedUser == null) return BadRequest("User konnte nicht geupdated werden");
            return Ok(updatedUser);
        }
        catch (Exception e)
        {
            // Log error and return a 500 Internal Server Error
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateUser)}");
        }
    }

    // Delete a user by their ID
    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult<bool>> DeleteUser(Guid userId)
    {
        try
        {
            var result = await userRepository.DeleteUserAsync(userId);
            return result ? Ok(true) : BadRequest("User konnte nicht gelöscht werden");
        }
        catch (Exception e)
        {
            // Log error and return a 500 Internal Server Error
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(DeleteUser)}");
        }
    }
}