using ToDoApi.Models;
using ToDoApi.Models.DTOs;

namespace ToDoApi.Services
{
    public class ToDoService : IToDoService
    {
        private readonly List<ToDo> _Tasks = new();
        private int _nextId = 1;

        public IEnumerable<ToDo> GetAll()
        {
            return _Tasks;
        }
        public ToDo GetById(int id)
        {
            return _Tasks.FirstOrDefault(t => t.Id == id);
        }
        public ToDo Create(CreateToDoDto input)
        {
            var task = new ToDo
            {
                Id = _nextId++,
                Title = input.Title,
                Description = input.Description,
                Status = false
            };
            _Tasks.Add(task);
            return task;
        }

        public ToDo Update(int id, UpdateToDoDto input)
        {
            var task = GetById(id);
            if (task == null) return null;
            task.Title = input.Title;
            task.Description = input.Description;
            task.Status = input.Status;
            return task;
        }

        public bool Delete(int id) 
        { 
            var task = GetById(id);
            if (task == null) return false;
            _Tasks.Remove(task);
            return true;


        }
    }
}

