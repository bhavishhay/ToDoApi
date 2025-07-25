using ToDoApi.Application.QueryParameters;
using ToDoApi.Domain.Entities; 
using ToDoApi.Application.DTOs;

namespace ToDoApi.Application.Interfaces.Services
{
    public interface IToDoService
    {
        Task<IEnumerable<ToDo>> GetAllAsync(ToDoQuaryFilterSortingParameters QuaryParameters);
        Task<ToDo> GetByIdAsync(int id);
        Task<ToDo> CreateAsync(CreateToDoDto input);
        Task<ToDo> UpdateAsync(int id, UpdateToDoDto input);
        Task<bool> DeleteAsync(int id);
    }
}
