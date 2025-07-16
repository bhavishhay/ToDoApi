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
        // private int _nextId = 1;

        public async Task<IEnumerable<ToDo>> GetAllAsync(ToDoQuaryFilterSortingParameters QuaryParameters)
        {
            var query = _context.ToDos.AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(QuaryParameters.Title))
            {
                query = query.Where(t => t.Title.Contains(QuaryParameters.Title));
            }
            if (QuaryParameters.Status.HasValue)
            {
                query = query.Where(t => t.Status == QuaryParameters.Status.Value);
            }

            // Sorting
            if (!string.IsNullOrEmpty(QuaryParameters.SortBy))
            {
                switch(QuaryParameters.SortBy)
                {   
                    case "Title":
                        query = QuaryParameters.SortDescending.HasValue && QuaryParameters.SortDescending.Value
                            ? query.OrderByDescending(t => t.Title)
                            : query.OrderBy(t => t.Title);
                        break;

                    case "Status":
                        query = QuaryParameters.SortDescending.HasValue && QuaryParameters.SortDescending.Value
                            ? query.OrderByDescending(t => t.Status)
                            : query.OrderBy(t => t.Status);
                        break;

                    case "Id":
                        query = QuaryParameters.SortDescending.HasValue && QuaryParameters.SortDescending.Value
                            ? query.OrderByDescending(t => t.Id)
                            : query.OrderBy(t => t.Id);
                        break;

                    default:
                        
                        break;
                }
                if((string.IsNullOrEmpty(QuaryParameters.SortBy) && (QuaryParameters.SortDescending == true)))
                {
                    query = query.OrderByDescending(t => t.Id);
                }

                // Note: The following commented code is an alternative way to handle sorting using EF.Property which allows dynamic property access. Suggested by visual studio 2022.
                //if (QuaryParameters.SortDescending.HasValue && QuaryParameters.SortDescending.Value)
                //{
                //    query = query.OrderByDescending(t => EF.Property<object>(t, QuaryParameters.SortBy));
                //}
                //else
                //{
                //    query = query.OrderBy(t => EF.Property<object>(t, QuaryParameters.SortBy));
                //}
            }

            // Paging
            query = query
                .Skip((QuaryParameters.PageNumber - 1) * QuaryParameters.PageSize)
                .Take(QuaryParameters.PageSize);

            return await query.ToListAsync();
        }    


        public async Task<ToDo> GetByIdAsync(int id)
        {
            return await _context.ToDos.FindAsync(id);
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

