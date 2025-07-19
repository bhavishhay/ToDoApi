using ToDoApi.Core.Entities;
using ToDoApi.Application.QueryParameters;

namespace ToDoApi.Application.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync(UserQuaryFilterSortingParameters queryParameters);
        Task<User> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
       // Task<bool> ExistsAsync(int id);
    }
}
