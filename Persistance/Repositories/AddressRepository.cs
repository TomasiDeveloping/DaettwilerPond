using Application.DataTransferObjects.Address;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly DaettwilerPondDbContext _context;
    private readonly IMapper _mapper;

    public AddressRepository(DaettwilerPondDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AddressDto>> GetAddressesAsync()
    {
        var addresses = await _context.Addresses
            .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return addresses;
    }

    public async Task<List<AddressDto>> GetUserAddressesAsync(Guid userId)
    {
        var userAddresses = await _context.Addresses
            .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .Where(a => a.UserId == userId)
            .ToListAsync();
        return userAddresses;
    }

    public async Task<AddressDto> GetAddressAsync(Guid addressId)
    {
        var address = await _context.Addresses
            .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == addressId);
        return address;
    }

    public async Task<AddressDto> CreateAddressAsync(AddressDto addressDto)
    {
        var address = _mapper.Map<Address>(addressDto);
        address.Id = Guid.NewGuid();
        await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();
        return _mapper.Map<AddressDto>(address);
    }

    public async Task<AddressDto> UpdateAddressAsync(Guid addressId, AddressDto addressDto)
    {
        var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == addressId);
        if (address == null) return null;
        _mapper.Map(addressDto, address);
        await _context.SaveChangesAsync();
        return _mapper.Map<AddressDto>(address);
    }

    public async Task<bool> DeleteAddressAsync(Guid addressId)
    {
        var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == addressId);
        if (address == null) return false;
        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();
        return true;
    }
}