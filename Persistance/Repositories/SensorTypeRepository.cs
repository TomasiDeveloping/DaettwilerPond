using Application.DataTransferObjects.SensorType;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class SensorTypeRepository : ISensorTypeRepository
{
    private readonly DaettwilerPondDbContext _context;
    private readonly IMapper _mapper;

    public SensorTypeRepository(DaettwilerPondDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<SensorTypeDto>> GetSensorTypesAsync()
    {
        var sensorTypes = await _context.SensorTypes
            .ProjectTo<SensorTypeDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return sensorTypes;
    }

    public async Task<SensorTypeDto> GetSensorTypeByIdAsync(Guid sensorTypeId)
    {
        var sensorType = await _context.SensorTypes
            .ProjectTo<SensorTypeDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(st => st.Id == sensorTypeId);
        return sensorType;
    }

    public async Task<SensorTypeDto> CreateSensorTypeAsync(CreateSensorTypeDto createSensorTypeDto)
    {
        var sensorType = _mapper.Map<SensorType>(createSensorTypeDto);
        await _context.SensorTypes.AddAsync(sensorType);
        await _context.SaveChangesAsync();
        return _mapper.Map<SensorTypeDto>(sensorType);
    }

    public async Task<SensorTypeDto> UpdateSensorTypeAsync(SensorTypeDto sensorTypeDto)
    {
        var sensorType = await _context.SensorTypes.FirstOrDefaultAsync(st => st.Id == sensorTypeDto.Id);
        if (sensorType == null) return null;
        _mapper.Map(sensorTypeDto, sensorType);
        await _context.SaveChangesAsync();
        return _mapper.Map<SensorTypeDto>(sensorType);
    }

    public async Task<bool> DeleteSensorTypeAsync(Guid sensorTypeId)
    {
        var sensorType = await _context.SensorTypes.FirstOrDefaultAsync(st => st.Id == sensorTypeId);
        if (sensorType == null) return false;
        _context.SensorTypes.Remove(sensorType);
        await _context.SaveChangesAsync();
        return true;
    }
}