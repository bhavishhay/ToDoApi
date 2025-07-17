using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;
using ToDoApi.Models.DTOs;
using ToDoApi.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ToDoApi.Services
{
    public class UserService: IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }
        //private int _nextId = 1;
        public async Task<IEnumerable<User>> GetAllAsync(UserQuaryFilterSortingParameters QuaryParameters)
        {
            var query = _context.Users.AsNoTracking().AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(QuaryParameters.Name))
            {
                query = query.Where(t => t.Name.Contains(QuaryParameters.Name));
                //query = query.Where(t => EF.Functions.Like(t.Name, $"%{QuaryParameters.Name}%")); //alternative way to filter using EF.Functions.Like

            }
            if (!string.IsNullOrEmpty(QuaryParameters.Email))
            {
                query = query.Where(t => t.Email.Contains(QuaryParameters.Email));
                //query = query.Where(t => EF.Functions.Like(t.Email, $"%{QuaryParameters.Email}%")); //alternative way to filter using EF.Functions.Like
            }
            if (!string.IsNullOrEmpty(QuaryParameters.Address))
            {
                query = query.Where(t => t.Address.Contains(QuaryParameters.Address));
                //query = query.Where(t => EF.Functions.Like(t.Address, $"%{QuaryParameters.Address}%"));  //alternative way to filter using EF.Functions.Like
            }

            // Sorting
            if (!string.IsNullOrEmpty(QuaryParameters.SortBy))
            {
                switch (QuaryParameters.SortBy)
                {
                    case "Name":
                        query = QuaryParameters.SortDescending.HasValue && QuaryParameters.SortDescending.Value
                            ? query.OrderByDescending(t => t.Name)
                            : query.OrderBy(t => t.Name);
                        break;

                    case "Email":
                        query = QuaryParameters.SortDescending.HasValue && QuaryParameters.SortDescending.Value
                            ? query.OrderByDescending(t => t.Email)
                            : query.OrderBy(t => t.Email);
                        break;

                    case "Address":
                        query = QuaryParameters.SortDescending.HasValue && QuaryParameters.SortDescending.Value
                            ? query.OrderByDescending(t => t.Address)
                            : query.OrderBy(t => t.Address);
                        break;

                    case "UserId":
                        query = QuaryParameters.SortDescending.HasValue && QuaryParameters.SortDescending.Value
                            ? query.OrderByDescending(t => t.UserId)
                            : query.OrderBy(t => t.UserId);
                        break;

                    default:

                        break;
                }
                if ((string.IsNullOrEmpty(QuaryParameters.SortBy) && (QuaryParameters.SortDescending == true)))
                {
                    query = query.OrderByDescending(t => t.UserId);
                }
            }

            // Paging
            query = query
            .Skip((QuaryParameters.PageNumber - 1) * QuaryParameters.PageSize)
            .Take(QuaryParameters.PageSize);

            Console.WriteLine(query.ToQueryString());

            return await query.ToListAsync();
        }
        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User> CreateAsync(CreateUserDto input)
        {
            var user = new User
            {
               // UserId = _nextId++,
                Name = input.Name,
                Email = input.Email,
                Address = input.Address
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> UpdateAsync(int id, UpdateUserDto input)
        {
            var user = await GetByIdAsync(id);
            if (user == null) return null;

            user.Name = input.Name;
            user.Email = input.Email;
            user.Address = input.Address;

            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
