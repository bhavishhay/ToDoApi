using MediatR;
using Moq;
using System.Linq.Expressions;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Application.QueryParameters;
using ToDoApi.Domain.Entities;
using ToDoApi.UnitTests.Fixtures;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ToDoApi.UnitTests.Helpers
{
    public static class MockRepositories
    {
        public static Mock<IToDoRepository> GetToDoRepository()
        {
            var todos = TestDataFactory.GetSampleToDos().ToList(); // Get a list of sample ToDos

            var mockRepo = new Mock<IToDoRepository>();

            // Mock GetAllAsync
            mockRepo.Setup(r => r.GetAllAsync(It.IsAny<ToDoQuaryFilterSortingParameters>()))
                    .ReturnsAsync((ToDoQuaryFilterSortingParameters queryParams) => {
                        var filteredTodos = todos.AsQueryable();

                        if (!string.IsNullOrEmpty(queryParams.Title))
                        {
                            filteredTodos = filteredTodos.Where(t => t.Title.Contains(queryParams.Title, StringComparison.OrdinalIgnoreCase));
                        }
                        if (queryParams.Status.HasValue)
                        {
                            filteredTodos = filteredTodos.Where(t => t.Status == queryParams.Status.Value);
                        }

                        // Basic sorting (you might need more robust sorting in a real app)
                        if (!string.IsNullOrEmpty(queryParams.SortBy))
                        {
                            Expression<Func<ToDo, object>> keySelector = queryParams.SortBy.ToLower() switch
                            {
                                "title" => t => t.Title,
                                "id" => t => t.Id,
                                "status" => t => t.Status,
                                _ => t => t.Id // Default sort by Id
                            };

                            filteredTodos = queryParams.SortDescending == true
                                ? filteredTodos.OrderByDescending(keySelector)
                                : filteredTodos.OrderBy(keySelector);
                        }
                        else
                        {
                            filteredTodos = filteredTodos.OrderBy(t => t.Id); // Default sort
                        }

                        return filteredTodos
                                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                                .Take(queryParams.PageSize)
                                .ToList();
                    });


            // Mock GetByIdAsync
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((int id) => todos.FirstOrDefault(t => t.Id == id));

            // Mock AddAsync
            mockRepo.Setup(r => r.AddAsync(It.IsAny<ToDo>()))
                    .ReturnsAsync((ToDo todo) => {
                        // Simulate adding to a database by assigning a new ID
                        todo.Id = todos.Any() ? todos.Max(t => t.Id) + 1 : 1;
                        todos.Add(todo);
                        return todo;
                    });

            // Mock UpdateAsync
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<ToDo>()))
                    .ReturnsAsync((ToDo todo) => {
                        var existingToDo = todos.FirstOrDefault(t => t.Id == todo.Id);
                        if (existingToDo != null)
                        {
                            existingToDo.Title = todo.Title;
                            existingToDo.Description = todo.Description;
                            existingToDo.Status = todo.Status;
                            return existingToDo;
                        }
                        return null; // Simulate not found
                    });

            // Mock DeleteAsync
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<int>()))
                    .ReturnsAsync((int id) => {
                        var todoToRemove = todos.FirstOrDefault(t => t.Id == id);
                        if (todoToRemove != null)
                        {
                            todos.Remove(todoToRemove);
                            return true;
                        }
                        return false; // Simulate not found
                    });

            return mockRepo;
        }

        public static Mock<IUserRepository> GetUserRepository()
        {
            var users = TestDataFactory.GetSampleUsers().ToList(); // Get a list of sample Users

            var mockRepo = new Mock<IUserRepository>();

           // Mock GetAllAsync
            mockRepo.Setup(r => r.GetAllAsync(It.IsAny<UserQuaryFilterSortingParameters>()))
                    .ReturnsAsync((UserQuaryFilterSortingParameters queryParams) => {
                        var filteredUsers = users.AsQueryable();

                        if (!string.IsNullOrEmpty(queryParams.Name))
                        {
                            filteredUsers = filteredUsers.Where(u => u.Name.Contains(queryParams.Name, StringComparison.OrdinalIgnoreCase));
                        }
                        if (!string.IsNullOrEmpty(queryParams.Email))
                        {
                            filteredUsers = filteredUsers.Where(u => u.Email.Contains(queryParams.Email, StringComparison.OrdinalIgnoreCase));
                        }
                        if (!string.IsNullOrEmpty(queryParams.Address))
                        {
                            filteredUsers = filteredUsers.Where(u => u.Address.Contains(queryParams.Address, StringComparison.OrdinalIgnoreCase));
                        }

                        // Basic sorting
                        if (!string.IsNullOrEmpty(queryParams.SortBy))
                        {
                            Expression<Func<User, object>> keySelector = queryParams.SortBy.ToLower() switch
                            {
                                "name" => u => u.Name,
                                "userid" => u => u.UserId,
                                "email" => u => u.Email,
                                "address" => u => u.Address,
                                _ => u => u.UserId // Default sort by UserId
                            };

                            filteredUsers = queryParams.SortDescending == true
                                ? filteredUsers.OrderByDescending(keySelector)
                                : filteredUsers.OrderBy(keySelector);
                        }
                        else
                        {
                            filteredUsers = filteredUsers.OrderBy(u => u.UserId); // Default sort
                        }

                        return filteredUsers
                                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                                .Take(queryParams.PageSize)
                                .ToList();
                    });

            // Mock GetByIdAsync
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((int id) => users.FirstOrDefault(u => u.UserId == id));

            // Mock AddAsync
            mockRepo.Setup(r => r.AddAsync(It.IsAny<User>()))
                    .ReturnsAsync((User user) => {
                        user.UserId = users.Any() ? users.Max(u => u.UserId) + 1 : 1;
                        users.Add(user);
                        return user;
                    });

            // Mock UpdateAsync
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<User>()))
                    .ReturnsAsync((User user) => {
                        var existingUser = users.FirstOrDefault(u => u.UserId == user.UserId);
                        if (existingUser != null)
                        {
                            existingUser.Name = user.Name;
                            existingUser.Email = user.Email;
                            existingUser.Address = user.Address;
                            return existingUser;
                        }
                        return null;
                    });

            // Mock DeleteAsync
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<int>()))
                    .ReturnsAsync((int id) => {
                        var userToRemove = users.FirstOrDefault(u => u.UserId == id);
                        if (userToRemove != null)
                        {
                            users.Remove(userToRemove);
                            return true;
                        }
                        return false;
                    });

            return mockRepo;
        }

        public static Mock<IMediator> GetMediator()
        {
            return new Mock<IMediator>();
        }
    }
}