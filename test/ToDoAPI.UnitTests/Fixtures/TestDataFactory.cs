using ToDoApi.Domain.Entities;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.Features.ToDos.Commands;
using ToDoApi.Application.Features.ToDos.Queries;
using ToDoApi.Application.Features.Users.Commands;
using ToDoApi.Application.Features.Users.Queries;
using ToDoApi.Application.QueryParameters;

namespace ToDoApi.UnitTests.Fixtures
{
    public static class TestDataFactory
    {
        public static List<ToDo> GetSampleToDos()
        {
            return new List<ToDo>
            {
                new ToDo { Id = 1, Title = "Buy Groceries", Description = "Milk, Eggs, Bread", Status = false },
                new ToDo { Id = 2, Title = "Clean House", Description = "Vacuum, Dust, Mop", Status = true },
                new ToDo { Id = 3, Title = "Finish Report", Description = "Complete Q3 Sales Report", Status = false },
                new ToDo { Id = 4, Title = "Call Mom", Description = "Wish her happy birthday", Status = false },
                new ToDo { Id = 5, Title = "Code Review", Description = "Review PR #123", Status = true }
            };
        }

        public static List<User> GetSampleUsers()
        {
            return new List<User>
            {
                new User { UserId = 1, Name = "Alice Smith", Email = "alice@example.com", Address = "123 Main St" },
                new User { UserId = 2, Name = "Bob Johnson", Email = "bob@example.com", Address = "456 Oak Ave" },
                new User { UserId = 3, Name = "Charlie Brown", Email = "charlie@example.com", Address = "789 Pine Ln" }
            };
        }

        // DTOs for Commands
        public static CreateToDoDto GetCreateToDoDto(string title = "New Task", string description = "New Task Description")
        {
            return new CreateToDoDto { Title = title, Description = description };
        }

        public static UpdateToDoDto GetUpdateToDoDto(string title = "Updated Task", string description = "Updated Task Description", bool status = true)
        {
            return new UpdateToDoDto { Title = title, Description = description, Status = status };
        }

        public static CreateUserDto GetCreateUserDto(string name = "New User", string email = "new.user@example.com", string address = "101 New Road")
        {
            return new CreateUserDto { Name = name, Email = email, Address = address };
        }

        public static UpdateUserDto GetUpdateUserDto(string name = "Updated User", string email = "updated.user@example.com", string address = "202 Updated Road")
        {
            return new UpdateUserDto { Name = name, Email = email, Address = address };
        }

        // Commands
        public static CreateToDoCommand GetCreateToDoCommand(string title = "New Task", string description = "New Task Description")
        {
            return new CreateToDoCommand(GetCreateToDoDto(title, description));
        }

        public static UpdateToDoCommand GetUpdateToDoCommand(int id = 1, string title = "Updated Task", string description = "Updated Task Description", bool status = true)
        {
            return new UpdateToDoCommand(id, GetUpdateToDoDto(title, description, status));
        }

        public static DeleteToDoCommand GetDeleteToDoCommand(int id = 1)
        {
            return new DeleteToDoCommand(id);
        }

        public static CreateUserCommand GetCreateUserCommand(string name = "New User", string email = "new.user@example.com", string address = "101 New Road")
        {
            return new CreateUserCommand(GetCreateUserDto(name, email, address));
        }

        public static UpdateUserCommand GetUpdateUserCommand(int id = 1, string name = "Updated User", string email = "updated.user@example.com", string address = "202 Updated Road")
        {
            return new UpdateUserCommand(id, GetUpdateUserDto(name, email, address));
        }

        public static DeleteUserCommand GetDeleteUserCommand(int id = 1)
        {
            return new DeleteUserCommand(id);
        }

        // Queries
        public static GetAllToDosQuery GetGetAllToDosQuery(int pageNumber = 1, int pageSize = 10, string? title = null, bool? status = null, string? sortBy = null, bool? sortDescending = false)
        {
            return new GetAllToDosQuery(new ToDoQuaryFilterSortingParameters
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Title = title,
                Status = status,
                SortBy = sortBy,
                SortDescending = sortDescending
            });
        }

        public static GetToDoByIdQuery GetGetToDoByIdQuery(int id = 1)
        {
            return new GetToDoByIdQuery(id);
        }

        public static GetAllUsersQuery GetGetAllUsersQuery(int pageNumber = 1, int pageSize = 10, string? name = null, string? email = null, string? address = null, string? sortBy = null, bool? sortDescending = false)
        {
            return new GetAllUsersQuery(new UserQuaryFilterSortingParameters
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Name = name,
                Email = email,
                Address = address,
                SortBy = sortBy,
                SortDescending = sortDescending
            });
        }

        public static GetUserByIdQuery GetGetUserByIdQuery(int id = 1)
        {
            return new GetUserByIdQuery(id);
        }
    }
}