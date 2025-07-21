using ToDoApi.Application.QueryParameters;
using ToDoApi.Domain.Entities; 
using ToDoApi.Application.DTOs;

namespace ToDoApi.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync(UserQuaryFilterSortingParameters QuaryParameters);
        Task<User> GetByIdAsync(int id);
        Task<User> CreateAsync(CreateUserDto input);
        Task<User> UpdateAsync(int id, UpdateUserDto input);
        Task<bool> DeleteAsync(int id);
    }
}
