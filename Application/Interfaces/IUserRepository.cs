﻿using Application.DataTransferObjects.User;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<UserDto> GetUserByIdAsync(Guid userId);
}