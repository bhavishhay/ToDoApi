using Microsoft.EntityFrameworkCore;
//using System.Threading.Tasks;
using ToDoApi.Data;
using ToDoApi.Models;
using ToDoApi.Models.DTOs;
using ToDoApi.Services.Interfaces;

namespace ToDoApi.Services
{
    public class ToDoService : IToDoService
    {
        private readonly AppDbContext _context;

        public ToDoService(AppDbContext context)
        {
            _context = context;
        }
        private int _nextId = 1;

        public async Task<IEnumerable<ToDo>> GetAllAsync()
        {
            return await _context.ToDos.ToListAsync();
        }
        public async Task<ToDo> GetByIdAsync(int id)
        {
            return await _context.ToDos.FindAsync(id);
        }
        public async Task<ToDo> CreateAsync(CreateToDoDto input)
        {
            var task = new ToDo
            {
                Id = _nextId++,
                Title = input.Title,
                Description = input.Description,
                Status = false
            };
            _context.ToDos.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<ToDo> UpdateAsync(int id, UpdateToDoDto input)
        {
            var task = await GetByIdAsync(id);
            if (task == null) return null;

            task.Title = input.Title;
            task.Description = input.Description;
            task.Status = input.Status;

            // _context.ToDos.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteAsync(int id) 
        { 
            var task = await GetByIdAsync(id);
            if (task == null) return false;
            _context.ToDos.Remove(task);
            await _context.SaveChangesAsync();
            return true;


        }
    }
}

