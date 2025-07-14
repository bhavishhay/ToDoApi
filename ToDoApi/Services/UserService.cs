using ToDoApi.Models;
using ToDoApi.Models.DTOs;
using ToDoApi.Services.Interfaces;

namespace ToDoApi.Services
{
    public class UserService: IUserService
    {
        private readonly List<User> _Users = new();
        private int _nextId = 1;
        public IEnumerable<User> GetAll()
        {
            return _Users;
        }
        public User GetById(int id)
        {
            return _Users.FirstOrDefault(u => u.UserId == id);
        }
        public User Create(CreateUserDto input)
        {
            var user = new User
            {
                UserId = _nextId++,
                Name = input.Name,
                Email = input.Email,
                Address = input.Address
            };
            _Users.Add(user);
            return user;
        }
        public User Update(int id, UpdateUserDto input)
        {
            var user = GetById(id);
            if (user == null) return null;
            user.Name = input.Name;
            user.Email = input.Email;
            user.Address = input.Address;
            return user;
        }
        public bool Delete(int id)
        {
            var user = GetById(id);
            if (user == null) return false;
            _Users.Remove(user);
            return true;
        }
    }
}
