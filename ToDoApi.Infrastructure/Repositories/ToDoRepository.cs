using Microsoft.EntityFrameworkCore;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Application.QueryParameters;
using ToDoApi.Core.Entities;
using ToDoApi.Infrastructure.Data;


namespace ToDoApi.Infrastructure.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly AppDbContext _context;
        public ToDoRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ToDo>> GetAllAsync(ToDoQuaryFilterSortingParameters quaryParameters)
        {
            var query = _context.ToDos.AsNoTracking().AsQueryable();
            // Filtering
            if (!string.IsNullOrEmpty(quaryParameters.Title))
            {
                query = query.Where(t => t.Title.Contains(quaryParameters.Title));
                //query = query.Where(t => EF.Functions.Like(t.Title, $"%{QuaryParameters.Title}%")); //alternative way to filter using EF.Functions.Like

            }
            if (quaryParameters.Status.HasValue)
            {
                query = query.Where(t => t.Status == quaryParameters.Status.Value);

            }
            // Sorting
            if (!string.IsNullOrEmpty(quaryParameters.SortBy))
            {
                switch (quaryParameters.SortBy)
                {
                    case "Title":
                        query = quaryParameters.SortDescending.HasValue && quaryParameters.SortDescending.Value
                            ? query.OrderByDescending(t => t.Title)
                            : query.OrderBy(t => t.Title);
                        break;
                    case "Status":
                        query = quaryParameters.SortDescending.HasValue && quaryParameters.SortDescending.Value
                            ? query.OrderByDescending(t => t.Status)
                            : query.OrderBy(t => t.Status);
                        break;
                    case "Id":
                        query = quaryParameters.SortDescending.HasValue && quaryParameters.SortDescending.Value
                            ? query.OrderByDescending(t => t.Id)
                            : query.OrderBy(t => t.Id);
                        break;
                    default:
                        break;
                }
                if ((string.IsNullOrEmpty(quaryParameters.SortBy) && (quaryParameters.SortDescending == true)))
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
            // Pagination
            query = query.Skip((quaryParameters.PageNumber - 1) * quaryParameters.PageSize)
                             .Take(quaryParameters.PageSize);

            Console.WriteLine(query.ToQueryString());

            return await query.ToListAsync();
        }
        public async Task<ToDo> GetByIdAsync(int id)
        {
            return await _context.ToDos.FindAsync(id);
        }
        public async Task<ToDo> AddAsync(ToDo todo)
        {
            _context.ToDos.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }
        public async Task<ToDo> UpdateAsync(ToDo todo)
        {
            _context.ToDos.Update(todo);
            await _context.SaveChangesAsync();
            return todo;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var todo = await _context.ToDos.FindAsync(id);
            if (todo == null) return false;
            _context.ToDos.Remove(todo);
            await _context.SaveChangesAsync();
            return true;
        }
       
    }
}
