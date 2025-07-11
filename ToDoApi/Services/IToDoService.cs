using ToDoApi.Models;
using ToDoApi.Models.DTOs;

namespace ToDoApi.Services
{
    public interface IToDoService
    {
        IEnumerable<ToDo> GetAll();
        ToDo GetById(int id);
        ToDo Create(CreateToDoDto input);
        ToDo Update(int id, UpdateToDoDto input);
        bool Delete(int id);
    }
}
