using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Api.Controllers; 
using ToDoApi.Application.Features.ToDos.Commands;
using ToDoApi.Application.Features.ToDos.Queries;
using ToDoApi.Domain.Entities;
using ToDoApi.Domain; 
using ToDoApi.UnitTests.Helpers; 
using ToDoApi.UnitTests.Fixtures; 


namespace ToDoApi.UnitTests.Controllers
{
    public class ToDoControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly ToDoController _controller;

        public ToDoControllerTests()
        {
            _mockMediator = MockRepositories.GetMediator();
            _controller = new ToDoController(_mockMediator.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithAllToDos_WhenToDosExist()
        {
            // Arrange
            var expectedToDos = TestDataFactory.GetSampleToDos();
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAllToDosQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(expectedToDos);

            // Act
            var result = await _controller.GetAll(TestDataFactory.GetGetAllToDosQuery().QueryParameters);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<ToDo>>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(" All Tasks fetched successfully", apiResponse.Message);
            Assert.Equal(expectedToDos.Count, apiResponse.Data.Count());
            _mockMediator.Verify(m => m.Send(It.IsAny<GetAllToDosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithEmptyList_WhenNoToDosFound()
        {
            // Arrange
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAllToDosQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<ToDo>());

            // Act
            var result = await _controller.GetAll(TestDataFactory.GetGetAllToDosQuery().QueryParameters);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<ToDo>>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal("No Task Found - list is empty", apiResponse.Message);
            Assert.Empty(apiResponse.Data);
            _mockMediator.Verify(m => m.Send(It.IsAny<GetAllToDosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkWithToDo_WhenToDoExists()
        {
            // Arrange
            var existingToDo = TestDataFactory.GetSampleToDos().First();
            _mockMediator.Setup(m => m.Send(It.IsAny<GetToDoByIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(existingToDo);

            // Act
            var result = await _controller.GetById(existingToDo.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<ToDo>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal("Task found and fetched successfully", apiResponse.Message);
            Assert.Equal(existingToDo.Id, apiResponse.Data.Id);
            _mockMediator.Verify(m => m.Send(It.Is<GetToDoByIdQuery>(q => q.Id == existingToDo.Id), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenToDoDoesNotExist()
        {
            // Arrange
            var nonExistentId = 999;
            _mockMediator.Setup(m => m.Send(It.IsAny<GetToDoByIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((ToDo?)null);

            // Act
            var result = await _controller.GetById(nonExistentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<ToDo>>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Task not found", apiResponse.Message);
            Assert.Null(apiResponse.Data);
            _mockMediator.Verify(m => m.Send(It.Is<GetToDoByIdQuery>(q => q.Id == nonExistentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtActionWithNewToDo()
        {
            // Arrange
            var createDto = TestDataFactory.GetCreateToDoDto();
            var newToDo = new ToDo { Id = 10, Title = createDto.Title, Description = createDto.Description, Status = false };
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateToDoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(newToDo);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<ToDo>>(createdAtActionResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal("New Task created successfully", apiResponse.Message);
            Assert.Equal(newToDo.Id, apiResponse.Data.Id);
            Assert.Equal(nameof(ToDoController.GetById), createdAtActionResult.ActionName);
            Assert.Equal(newToDo.Id, createdAtActionResult.RouteValues?["id"]);
            _mockMediator.Verify(m => m.Send(It.Is<CreateToDoCommand>(cmd => cmd.ToDoDto == createDto), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnOkWithUpdatedToDo_WhenToDoExists()
        {
            // Arrange
            var existingToDoId = 1;
            var updateDto = TestDataFactory.GetUpdateToDoDto("Updated Title", "Updated Desc", true);
            var updatedToDo = new ToDo { Id = existingToDoId, Title = updateDto.Title, Description = updateDto.Description, Status = updateDto.Status };
            _mockMediator.Setup(m => m.Send(It.IsAny<UpdateToDoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(updatedToDo);

            // Act
            var result = await _controller.Update(existingToDoId, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<ToDo>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal("Task updated successfully", apiResponse.Message);
            Assert.Equal(existingToDoId, apiResponse.Data.Id);
            Assert.Equal("Updated Title", apiResponse.Data.Title);
            _mockMediator.Verify(m => m.Send(It.Is<UpdateToDoCommand>(cmd => cmd.Id == existingToDoId && cmd.ToDoDto == updateDto), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenToDoDoesNotExist()
        {
            // Arrange
            var nonExistentId = 999;
            var updateDto = TestDataFactory.GetUpdateToDoDto();
            _mockMediator.Setup(m => m.Send(It.IsAny<UpdateToDoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((ToDo?)null);

            // Act
            var result = await _controller.Update(nonExistentId, updateDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<ToDo>>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Task not found", apiResponse.Message);
            Assert.Null(apiResponse.Data);
            _mockMediator.Verify(m => m.Send(It.Is<UpdateToDoCommand>(cmd => cmd.Id == nonExistentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenToDoIsDeleted()
        {
            // Arrange
            var existingToDoId = 1;
            _mockMediator.Setup(m => m.Send(It.IsAny<DeleteToDoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(existingToDoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal("Task deleted successfully", apiResponse.Message);
            _mockMediator.Verify(m => m.Send(It.Is<DeleteToDoCommand>(cmd => cmd.Id == existingToDoId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenToDoDoesNotExist()
        {
            // Arrange
            var nonExistentId = 999;
            _mockMediator.Setup(m => m.Send(It.IsAny<DeleteToDoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(nonExistentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("Task not found", apiResponse.Message);
            _mockMediator.Verify(m => m.Send(It.Is<DeleteToDoCommand>(cmd => cmd.Id == nonExistentId), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}