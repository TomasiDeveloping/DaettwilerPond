using Application.DataTransferObjects.Address;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

// Define the route, API version, and authorize the controller
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class AddressesController(IAddressRepository addressRepository, ILogger<AddressesController> logger)
    : ControllerBase
{
    // Handle GET request to retrieve all addresses
    [HttpGet]
    public async Task<ActionResult<List<AddressDto>>> GetAddresses()
    {
        try
        {
            var addresses = await addressRepository.GetAddressesAsync();
            return addresses.Count != 0 ? Ok(addresses) : NoContent();
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetAddresses)}");
        }
    }

    // Handle GET request to retrieve a specific address by ID
    [HttpGet("{addressId:guid}")]
    public async Task<ActionResult<AddressDto>> GetAddress(Guid addressId)
    {
        try
        {
            var address = await addressRepository.GetAddressAsync(addressId);
            if (address == null) return BadRequest($"Keine Adresse gefunden mit der Id: {addressId}");
            return Ok(address);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetAddress)}");
        }
    }

    // Handle GET request to retrieve addresses associated with a specific user
    [HttpGet("users/{userId:guid}")]
    public async Task<ActionResult<List<AddressDto>>> GetUserAddresses(Guid userId)
    {
        try
        {
            var userAddresses = await addressRepository.GetUserAddressesAsync(userId);
            return userAddresses.Count != 0 ? Ok(userAddresses) : NoContent();
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetUserAddresses)}");
        }
    }

    // Handle POST request to create a new address
    [HttpPost]
    public async Task<ActionResult<AddressDto>> CreateAddress(AddressDto addressDto)
    {
        try
        {
            var newAddress = await addressRepository.CreateAddressAsync(addressDto);
            return newAddress == null
                ? BadRequest("Neue Adresse konnte nicht erstellt werden")
                : StatusCode(StatusCodes.Status201Created, newAddress);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateAddress)}");
        }
    }

    // Handle PUT request to update an existing address by ID
    [HttpPut("{addressId:guid}")]
    public async Task<ActionResult<AddressDto>> UpdateAddress(Guid addressId, AddressDto addressDto)
    {
        try
        {
            // Validate ID match before updating
            if (addressId != addressDto.Id) return BadRequest("Fehler in Id's");

            var updatedAddress = await addressRepository.UpdateAddressAsync(addressId, addressDto);
            if (updatedAddress == null) return BadRequest("Adresse konnte nicht geupdated werden");
            return Ok(updatedAddress);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateAddress)}");
        }
    }
}