using Api.Domain.Dtos.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Domain.Interfaces.Services.User
{
    public interface IUserService
    {
        Task<UserDto> GetAsync(Guid id);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDtoCreateResult> PostAsync(UserDtoCreate userDto);
        Task<UserDtoUpdateResult> PutAsync(UserDtoUpdate userDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
