using Application.DataTransferObjects.Address;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class AddressesController(IAddressRepository addressRepository, ILogger<AddressesController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AddressDto>>> GetAddresses()
    {
        try
        {
            var addresses = await addressRepository.GetAddressesAsync();
            return addresses.Any() ? Ok(addresses) : NoContent();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetAddresses)}");
        }
    }

    [HttpGet("{addressId}")]
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
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetAddress)}");
        }
    }

    [HttpGet("users/{userId}")]
    public async Task<ActionResult<List<AddressDto>>> GetUserAddresses(Guid userId)
    {
        try
        {
            var userAddresses = await addressRepository.GetUserAddressesAsync(userId);
            return userAddresses.Any() ? Ok(userAddresses) : NoContent();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetUserAddresses)}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<AddressDto>> CreateAddress(AddressDto addressDto)
    {
        try
        {
            var newAddress = await addressRepository.CreateAddressAsync(addressDto);
            if (newAddress == null) return BadRequest("Neue Adresse konnte nicht erstellt werden");
            return StatusCode(StatusCodes.Status201Created, newAddress);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateAddress)}");
        }
    }

    [HttpPut("{addressId}")]
    public async Task<ActionResult<AddressDto>> UpdateAddress(Guid addressId, AddressDto addressDto)
    {
        try
        {
            if (addressId != addressDto.Id) return BadRequest("Fehler in Id's");
            var updatedAddress = await addressRepository.UpdateAddressAsync(addressId, addressDto);
            if (updatedAddress == null) return BadRequest("Adresse konnte nicht geupdated werden");
            return Ok(updatedAddress);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateAddress)}");
        }
    }
}