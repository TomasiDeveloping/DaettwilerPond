﻿using Application.DataTransferObjects.FishType;

namespace Application.Interfaces;

public interface IFishTypeRepository
{
    Task<List<FishTypeDto>> GetFishTypesAsync();
    Task<FishTypeDto> CreateFishTypeAsync(FishTypeDto fishTypeDto);
    Task<FishTypeDto> UpdateFishTypeAsync(Guid fishTypeId, FishTypeDto fishTypeDto);
    Task<bool> DeleteFishTypeAsync(Guid fishTypeId);
}