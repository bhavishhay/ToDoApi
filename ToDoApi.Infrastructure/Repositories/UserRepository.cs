using Microsoft.EntityFrameworkCore;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Application.QueryParameters;
using ToDoApi.Core.Entities;
using ToDoApi.Infrastructure.Data;

namespace ToDoApi.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllAsync(UserQuaryFilterSortingParameters queryParameters)
        {
            var query = _context.Users.AsNoTracking().AsQueryable();
            // Filtering
            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                query = query.Where(t => t.Name.Contains(queryParameters.Name));
                //query = query.Where(t => EF.Functions.Like(t.Name, $"%{QuaryParameters.Name}%")); //alternative way to filter using EF.Functions.Like

            }
            if (!string.IsNullOrEmpty(queryParameters.Email))
            {
                query = query.Where(t => t.Email.Contains(queryParameters.Email));
                //query = query.Where(t => EF.Functions.Like(t.Email, $"%{QuaryParameters.Email}%")); //alternative way to filter using EF.Functions.Like

            }
            if (!string.IsNullOrEmpty(queryParameters.Address))
            {
                query = query.Where(t => t.Address.Contains(queryParameters.Address));
                //query = query.Where(t => EF.Functions.Like(t.Address, $"%{QuaryParameters.Address}%"));  //alternative way to filter using EF.Functions.Like

            }
            // Sorting
            if (!string.IsNullOrEmpty(queryParameters.SortBy))
            {
                switch (queryParameters.SortBy)
                {
                    case "Name":
                        query = queryParameters.SortDescending.HasValue && queryParameters.SortDescending.Value
                            ? query.OrderByDescending(t => t.Name)
                            : query.OrderBy(t => t.Name);
                        break;
                    case "Email":
                        query = queryParameters.SortDescending.HasValue && queryParameters.SortDescending.Value
                            ? query.OrderByDescending(t => t.Email)
                            : query.OrderBy(t => t.Email);
                        break;
                    case "Address":
                        query = queryParameters.SortDescending.HasValue && queryParameters.SortDescending.Value
                            ? query.OrderByDescending(t => t.Address)
                            : query.OrderBy(t => t.Address);
                        break;
                    default:
                        break;
                }
                if ((string.IsNullOrEmpty(queryParameters.SortBy) && (queryParameters.SortDescending == true)))
                {
                    query = query.OrderByDescending(t => t.UserId);
                }
            }
            // Pagination
            query = query
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize);

            Console.WriteLine(query.ToQueryString());

            return await query.ToListAsync();
        }
        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
