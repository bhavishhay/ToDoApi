using ToDoApi.Application.DTOs;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Application.Interfaces.Services;
using ToDoApi.Application.QueryParameters;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Application.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
       
        public async Task<IEnumerable<User>> GetAllAsync(UserQuaryFilterSortingParameters QuaryParameters)
        {
            return await _repository.GetAllAsync(QuaryParameters);
        }
        public async Task<User> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
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

            return await _repository.AddAsync(user);
        }

        public async Task<User> UpdateAsync(int id, UpdateUserDto input)
        {
            var user = await GetByIdAsync(id);
            if (user == null) return null;

            user.Name = input.Name;
            user.Email = input.Email;
            user.Address = input.Address;

            return await _repository.UpdateAsync(user);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            // var user = await GetByIdAsync(id);
            // if (user == null) return false;
            return await _repository.DeleteAsync(id);
        }
    }
}
