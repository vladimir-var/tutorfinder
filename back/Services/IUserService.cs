using System.Collections.Generic;
using System.Threading.Tasks;
using tutorfinder.Models.DTOs;

namespace tutorfinder.Services
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> CreateAsync(CreateUserDto createUserDto);
        Task<UserDto> UpdateAsync(int id, UpdateUserDto updateUserDto);
        Task DeleteAsync(int id);
        Task<UserDto> GetByEmailAsync(string email);
    }
} 