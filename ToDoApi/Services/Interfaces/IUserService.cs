using ToDoApi.Models;
using ToDoApi.Models.DTOs;

namespace ToDoApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> CreateAsync(CreateUserDto input);
        Task<User> UpdateAsync(int id, UpdateUserDto input);
        Task<bool> DeleteAsync(int id);
    }
}
