using Application.DataTransferObjects.Address;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class AddressRepository(DaettwilerPondDbContext context, IMapper mapper) : IAddressRepository
{
    public async Task<List<AddressDto>> GetAddressesAsync()
    {
        var addresses = await context.Addresses
            .ProjectTo<AddressDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return addresses;
    }

    public async Task<List<AddressDto>> GetUserAddressesAsync(Guid userId)
    {
        var userAddresses = await context.Addresses
            .ProjectTo<AddressDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .Where(a => a.UserId == userId)
            .ToListAsync();
        return userAddresses;
    }

    public async Task<AddressDto> GetAddressAsync(Guid addressId)
    {
        var address = await context.Addresses
            .ProjectTo<AddressDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == addressId);
        return address;
    }

    public async Task<AddressDto> CreateAddressAsync(AddressDto addressDto)
    {
        var address = mapper.Map<Address>(addressDto);
        address.Id = Guid.NewGuid();
        await context.Addresses.AddAsync(address);
        await context.SaveChangesAsync();
        return mapper.Map<AddressDto>(address);
    }

    public async Task<AddressDto> UpdateAddressAsync(Guid addressId, AddressDto addressDto)
    {
        var address = await context.Addresses.FirstOrDefaultAsync(a => a.Id == addressId);
        if (address == null) return null;
        mapper.Map(addressDto, address);
        await context.SaveChangesAsync();
        return mapper.Map<AddressDto>(address);
    }

    public async Task<bool> DeleteAddressAsync(Guid addressId)
    {
        var address = await context.Addresses.FirstOrDefaultAsync(a => a.Id == addressId);
        if (address == null) return false;
        context.Addresses.Remove(address);
        await context.SaveChangesAsync();
        return true;
    }
}