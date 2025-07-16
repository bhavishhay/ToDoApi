using ToDoApi.Models;
using ToDoApi.Models.DTOs;

namespace ToDoApi.Services.Interfaces
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
