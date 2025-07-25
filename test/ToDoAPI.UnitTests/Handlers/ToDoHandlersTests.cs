using Moq;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Application.Features.ToDos.Handlers;
using ToDoApi.Domain.Entities;
using ToDoApi.UnitTests.Helpers;
using ToDoApi.UnitTests.Fixtures;
using ToDoApi.Application.QueryParameters;

namespace ToDoApi.UnitTests.Handlers
{
    public class ToDoHandlersTests
    {
        private readonly Mock<IToDoRepository> _mockToDoRepository;

        public ToDoHandlersTests()
        {
            _mockToDoRepository = MockRepositories.GetToDoRepository();
        }

        [Fact]
        public async Task CreateToDoCommandHandler_ShouldAddToDoAndReturnIt()
        {
            // Arrange
            var createCommand = TestDataFactory.GetCreateToDoCommand("Test Title", "Test Description");
            var handler = new CreateToDoCommandHandler(_mockToDoRepository.Object);

            // Act
            var result = await handler.Handle(createCommand, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Title", result.Title);
            Assert.Equal("Test Description", result.Description);
            Assert.False(result.Status); // Default status
            _mockToDoRepository.Verify(r => r.AddAsync(It.Is<ToDo>(t => t.Title == "Test Title")), Times.Once);
        }

        [Fact]
        public async Task GetToDoByIdQueryHandler_ShouldReturnToDo_WhenToDoExists()
        {
            // Arrange
            var existingToDoId = 1;
            var query = TestDataFactory.GetGetToDoByIdQuery(existingToDoId);
            var handler = new GetToDoByIdQueryHandler(_mockToDoRepository.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingToDoId, result.Id);
            Assert.Equal("Buy Groceries", result.Title); // From TestDataFactory
            _mockToDoRepository.Verify(r => r.GetByIdAsync(existingToDoId), Times.Once);
        }

        [Fact]
        public async Task GetToDoByIdQueryHandler_ShouldReturnNull_WhenToDoDoesNotExist()
        {
            // Arrange
            var nonExistentToDoId = 999;
            var query = TestDataFactory.GetGetToDoByIdQuery(nonExistentToDoId);
            var handler = new GetToDoByIdQueryHandler(_mockToDoRepository.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _mockToDoRepository.Verify(r => r.GetByIdAsync(nonExistentToDoId), Times.Once);
        }

        [Fact]
        public async Task UpdateToDoCommandHandler_ShouldUpdateAndReturnToDo_WhenToDoExists()
        {
            // Arrange
            var existingToDoId = 1;
            var updateCommand = TestDataFactory.GetUpdateToDoCommand(existingToDoId, "Updated Groceries", "Updated Description", true);
            var handler = new UpdateToDoCommandHandler(_mockToDoRepository.Object);

            // Act
            var result = await handler.Handle(updateCommand, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingToDoId, result.Id);
            Assert.Equal("Updated Groceries", result.Title);
            Assert.Equal("Updated Description", result.Description);
            Assert.True(result.Status);
            _mockToDoRepository.Verify(r => r.GetByIdAsync(existingToDoId), Times.Once); // Verify GetByIdAsync was called
            _mockToDoRepository.Verify(r => r.UpdateAsync(It.Is<ToDo>(t => t.Id == existingToDoId && t.Title == "Updated Groceries")), Times.Once); // Verify UpdateAsync was called
        }

        [Fact]
        public async Task UpdateToDoCommandHandler_ShouldReturnNull_WhenToDoDoesNotExist()
        {
            // Arrange
            var nonExistentToDoId = 999;
            var updateCommand = TestDataFactory.GetUpdateToDoCommand(nonExistentToDoId, "Non Existent", "Non Existent Desc", false);
            var handler = new UpdateToDoCommandHandler(_mockToDoRepository.Object);

            // Act
            var result = await handler.Handle(updateCommand, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _mockToDoRepository.Verify(r => r.GetByIdAsync(nonExistentToDoId), Times.Once);
            _mockToDoRepository.Verify(r => r.UpdateAsync(It.IsAny<ToDo>()), Times.Never); // Ensure UpdateAsync was NOT called
        }

        [Fact]
        public async Task DeleteToDoCommandHandler_ShouldReturnTrue_WhenToDoExists()
        {
            // Arrange
            var existingToDoId = 2; // Use an ID that exists in sample data
            var deleteCommand = TestDataFactory.GetDeleteToDoCommand(existingToDoId);
            var handler = new DeleteToDoCommandHandler(_mockToDoRepository.Object);

            // Act
            var result = await handler.Handle(deleteCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockToDoRepository.Verify(r => r.DeleteAsync(existingToDoId), Times.Once);
        }

        [Fact]
        public async Task DeleteToDoCommandHandler_ShouldReturnFalse_WhenToDoDoesNotExist()
        {
            // Arrange
            var nonExistentToDoId = 999;
            var deleteCommand = TestDataFactory.GetDeleteToDoCommand(nonExistentToDoId);
            var handler = new DeleteToDoCommandHandler(_mockToDoRepository.Object);

            // Act
            var result = await handler.Handle(deleteCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mockToDoRepository.Verify(r => r.DeleteAsync(nonExistentToDoId), Times.Once);
        }

        [Fact]
        public async Task GetAllToDosQueryHandler_ShouldReturnAllToDos()
        {
            // Arrange
            var query = TestDataFactory.GetGetAllToDosQuery(); // Default query params
            var handler = new GetAllToDosQueryHandler(_mockToDoRepository.Object);
            var expectedToDosCount = TestDataFactory.GetSampleToDos().Count(); // Total count in our mock

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedToDosCount, result.Count());
            _mockToDoRepository.Verify(r => r.GetAllAsync(It.IsAny<ToDoQuaryFilterSortingParameters>()), Times.Once);
        }

        [Fact]
        public async Task GetAllToDosQueryHandler_ShouldReturnFilteredToDos_ByTitle()
        {
            // Arrange
            var query = TestDataFactory.GetGetAllToDosQuery(title: "Report");
            var handler = new GetAllToDosQueryHandler(_mockToDoRepository.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Only one element returned "Finish Report"
            Assert.Equal("Finish Report", result.First().Title);
        }

        [Fact]
        public async Task GetAllToDosQueryHandler_ShouldReturnFilteredToDos_ByStatus()
        {
            // Arrange
            var query = TestDataFactory.GetGetAllToDosQuery(status: true);
            var handler = new GetAllToDosQueryHandler(_mockToDoRepository.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); // "Clean House", "Code Review"
            Assert.Contains(result, t => t.Title == "Clean House");
            Assert.Contains(result, t => t.Title == "Code Review");
        }

        [Fact]
        public async Task GetAllToDosQueryHandler_ShouldReturnSortedToDos_ByTitleAscending()
        {
            // Arrange
            var query = TestDataFactory.GetGetAllToDosQuery(sortBy: "title", sortDescending: false);
            var handler = new GetAllToDosQueryHandler(_mockToDoRepository.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            var titles = result.Select(t => t.Title).ToList();
            Assert.Equal(new List<string> { "Buy Groceries", "Call Mom", "Clean House", "Code Review", "Finish Report" }, titles);
        }

        [Fact]
        public async Task GetAllToDosQueryHandler_ShouldReturnPaginatedToDos()
        {
            // Arrange
            var query = TestDataFactory.GetGetAllToDosQuery(pageNumber: 2, pageSize: 2);
            var handler = new GetAllToDosQueryHandler(_mockToDoRepository.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); // Second page with 2 items
            // Based on default ID sort: Should be ToDo 3 and 4
            Assert.Equal(3, result.First().Id);
            Assert.Equal(4, result.Last().Id);
        }
    }
}