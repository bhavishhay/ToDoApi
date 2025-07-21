using ToDoApi.Application.DTOs;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Application.Interfaces.Services;
using ToDoApi.Application.QueryParameters;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Application.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _repository;

        public ToDoService(IToDoRepository repository)
        {
            _repository = repository;
        }
       

        public async Task<IEnumerable<ToDo>> GetAllAsync(ToDoQuaryFilterSortingParameters QuaryParameters)
        {
            return await _repository.GetAllAsync(QuaryParameters);
        }    


        public async Task<ToDo> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<ToDo> CreateAsync(CreateToDoDto input)
        {
            var task = new ToDo
            {
               // Id = _nextId++,
                Title = input.Title,
                Description = input.Description,
                Status = false
            };

            return await _repository.AddAsync(task);
        }

        public async Task<ToDo> UpdateAsync(int id, UpdateToDoDto input)
        {
            var task = await GetByIdAsync(id);
            if (task == null) return null;

            task.Title = input.Title;
            task.Description = input.Description;
            task.Status = input.Status;

            return await _repository.UpdateAsync(task);
        }

        public async Task<bool> DeleteAsync(int id) 
        { 
           // var task = await GetByIdAsync(id);
           // if (task == null) return false;
            return await _repository.DeleteAsync(id);
        }
    }
}

