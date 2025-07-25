using Moq;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Application.Features.Users.Handlers;
using ToDoApi.Domain.Entities;
using ToDoApi.UnitTests.Helpers;
using ToDoApi.UnitTests.Fixtures;

namespace ToDoApi.UnitTests.Handlers
{
    public class UserHandlersTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;

        public UserHandlersTests()
        {
            _mockUserRepository = MockRepositories.GetUserRepository();
        }

        [Fact]
        public async Task CreateUserCommandHandler_ShouldAddUserAndReturnIt()
        {
            // Arrange
            var createCommand = TestDataFactory.GetCreateUserCommand("Test User", "test@example.com", "123 Test St");
            var handler = new CreateUserCommandHandler(_mockUserRepository.Object);

            // Act
            var result = await handler.Handle(createCommand, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("test@example.com", result.Email);
            Assert.Equal("123 Test St", result.Address);
            _mockUserRepository.Verify(r => r.AddAsync(It.Is<User>(u => u.Email == "test@example.com")), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdQueryHandler_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var existingUserId = 1;
            var query = TestDataFactory.GetGetUserByIdQuery(existingUserId);
            var handler = new GetUserByIdQueryHandler(_mockUserRepository.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingUserId, result.UserId);
            Assert.Equal("Alice Smith", result.Name); // From TestDataFactory
            _mockUserRepository.Verify(r => r.GetByIdAsync(existingUserId), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdQueryHandler_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistentUserId = 999;
            var query = TestDataFactory.GetGetUserByIdQuery(nonExistentUserId);
            var handler = new GetUserByIdQueryHandler(_mockUserRepository.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _mockUserRepository.Verify(r => r.GetByIdAsync(nonExistentUserId), Times.Once);
        }

        [Fact]
        public async Task UpdateUserCommandHandler_ShouldUpdateAndReturnUser_WhenUserExists()
        {
            // Arrange
            var existingUserId = 1;
            var updateCommand = TestDataFactory.GetUpdateUserCommand(existingUserId, "Updated Alice", "updated.alice@example.com", "New Address");
            var handler = new UpdateUserCommandHandler(_mockUserRepository.Object);

            // Act
            var result = await handler.Handle(updateCommand, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingUserId, result.UserId);
            Assert.Equal("Updated Alice", result.Name);
            Assert.Equal("updated.alice@example.com", result.Email);
            Assert.Equal("New Address", result.Address);
            _mockUserRepository.Verify(r => r.GetByIdAsync(existingUserId), Times.Once);
            _mockUserRepository.Verify(r => r.UpdateAsync(It.Is<User>(u => u.UserId == existingUserId && u.Name == "Updated Alice")), Times.Once);
        }

        [Fact]
        public async Task UpdateUserCommandHandler_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistentUserId = 999;
            var updateCommand = TestDataFactory.GetUpdateUserCommand(nonExistentUserId, "Non Existent", "non.existent@example.com", "Nowhere");
            var handler = new UpdateUserCommandHandler(_mockUserRepository.Object);

            // Act
            var result = await handler.Handle(updateCommand, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _mockUserRepository.Verify(r => r.GetByIdAsync(nonExistentUserId), Times.Once);
            _mockUserRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task DeleteUserCommandHandler_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            var existingUserId = 2;
            var deleteCommand = TestDataFactory.GetDeleteUserCommand(existingUserId);
            var handler = new DeleteUserCommandHandler(_mockUserRepository.Object);

            // Act
            var result = await handler.Handle(deleteCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockUserRepository.Verify(r => r.DeleteAsync(existingUserId), Times.Once);
        }

        [Fact]
        public async Task DeleteUserCommandHandler_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistentUserId = 999;
            var deleteCommand = TestDataFactory.GetDeleteUserCommand(nonExistentUserId);
            var handler = new DeleteUserCommandHandler(_mockUserRepository.Object);

            // Act
            var result = await handler.Handle(deleteCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mockUserRepository.Verify(r => r.DeleteAsync(nonExistentUserId), Times.Once);
        }

        [Fact]
        public async Task GetAllUsersQueryHandler_ShouldReturnAllUsers()
        {
            // Arrange
            var query = TestDataFactory.GetGetAllUsersQuery();
            var handler = new GetAllUsersQueryHandler(_mockUserRepository.Object);
            var expectedUsersCount = TestDataFactory.GetSampleUsers().Count();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUsersCount, result.Count());
            _mockUserRepository.Verify(r => r.GetAllAsync(It.IsAny<ToDoApi.Application.QueryParameters.UserQuaryFilterSortingParameters>()), Times.Once);
        }

        [Fact]
        public async Task GetAllUsersQueryHandler_ShouldReturnFilteredUsers_ByName()
        {
            // Arrange
            var query = TestDataFactory.GetGetAllUsersQuery(name: "Bob");
            var handler = new GetAllUsersQueryHandler(_mockUserRepository.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Bob Johnson", result.First().Name);
        }

        [Fact]
        public async Task GetAllUsersQueryHandler_ShouldReturnSortedUsers_ByEmailDescending()
        {
            // Arrange
            var query = TestDataFactory.GetGetAllUsersQuery(sortBy: "email", sortDescending: true);
            var handler = new GetAllUsersQueryHandler(_mockUserRepository.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            var emails = result.Select(u => u.Email).ToList();
            Assert.Equal(new List<string> { "charlie@example.com", "bob@example.com", "alice@example.com" }, emails);
        }

        [Fact]
        public async Task GetAllUsersQueryHandler_ShouldReturnPaginatedUsers()
        {
            // Arrange
            var query = TestDataFactory.GetGetAllUsersQuery(pageNumber: 2, pageSize: 2);
            var handler = new GetAllUsersQueryHandler(_mockUserRepository.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Second page with 1 item (3 total, 2 per page)
            Assert.Equal(3, result.First().UserId);
        }
    }
}