using ToDoApi.Domain.Entities;
using ToDoApi.Application.QueryParameters;

namespace ToDoApi.Application.Interfaces.IRepositories
{
    public interface IToDoRepository
    {
        Task<IEnumerable<ToDo>> GetAllAsync(ToDoQuaryFilterSortingParameters quaryParameters);
        Task<ToDo> GetByIdAsync(int id);
        Task<ToDo> AddAsync(ToDo todo);
        Task<ToDo> UpdateAsync(ToDo todo);
        Task<bool> DeleteAsync(int id);
        
    }
}
