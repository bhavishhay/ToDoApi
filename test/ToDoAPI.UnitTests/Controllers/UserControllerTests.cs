using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Api.Controllers;
using ToDoApi.Application.Features.Users.Commands;
using ToDoApi.Application.Features.Users.Queries;
using ToDoApi.Domain.Entities;
using ToDoApi.Domain; 
using ToDoApi.UnitTests.Helpers; 
using ToDoApi.UnitTests.Fixtures; 


namespace ToDoApi.UnitTests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockMediator = MockRepositories.GetMediator();
            _controller = new UserController(_mockMediator.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithAllUsers_WhenUsersExist()
        {
            // Arrange
            var expectedUsers = TestDataFactory.GetSampleUsers();
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(expectedUsers);

            // Act
            var result = await _controller.GetAll(TestDataFactory.GetGetAllUsersQuery().QueryParameters);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<User>>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(" All Users fetched successfully", apiResponse.Message);
            Assert.Equal(expectedUsers.Count, apiResponse.Data.Count());
            _mockMediator.Verify(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithEmptyList_WhenNoUsersFound()
        {
            // Arrange
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<User>());

            // Act
            var result = await _controller.GetAll(TestDataFactory.GetGetAllUsersQuery().QueryParameters);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<User>>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal("No User Found - list is empty", apiResponse.Message);
            Assert.Empty(apiResponse.Data);
            _mockMediator.Verify(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkWithUser_WhenUserExists()
        {
            // Arrange
            var existingUser = TestDataFactory.GetSampleUsers().First();
            _mockMediator.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(existingUser);

            // Act
            var result = await _controller.GetById(existingUser.UserId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<User>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal("User found and fetched successfully", apiResponse.Message);
            Assert.Equal(existingUser.UserId, apiResponse.Data.UserId);
            _mockMediator.Verify(m => m.Send(It.Is<GetUserByIdQuery>(q => q.Id == existingUser.UserId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistentId = 999;
            _mockMediator.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((User?)null);

            // Act
            var result = await _controller.GetById(nonExistentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<User>>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("User not found", apiResponse.Message);
            Assert.Null(apiResponse.Data);
            _mockMediator.Verify(m => m.Send(It.Is<GetUserByIdQuery>(q => q.Id == nonExistentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtActionWithNewUser()
        {
            // Arrange
            var createDto = TestDataFactory.GetCreateUserDto();
            var newUser = new User { UserId = 10, Name = createDto.Name, Email = createDto.Email, Address = createDto.Address };
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(newUser);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<User>>(createdAtActionResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal("New User created successfully", apiResponse.Message);
            Assert.Equal(newUser.UserId, apiResponse.Data.UserId);
            Assert.Equal(nameof(UserController.GetById), createdAtActionResult.ActionName);
            Assert.Equal(newUser.UserId, createdAtActionResult.RouteValues?["id"]);
            _mockMediator.Verify(m => m.Send(It.Is<CreateUserCommand>(cmd => cmd.UserDto == createDto), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnOkWithUpdatedUser_WhenUserExists()
        {
            // Arrange
            var existingUserId = 1;
            var updateDto = TestDataFactory.GetUpdateUserDto("Updated Name", "updated.email@example.com", "Updated Address");
            var updatedUser = new User { UserId = existingUserId, Name = updateDto.Name, Email = updateDto.Email, Address = updateDto.Address };
            _mockMediator.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(updatedUser);

            // Act
            var result = await _controller.Update(existingUserId, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<User>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal("User updated successfully", apiResponse.Message);
            Assert.Equal(existingUserId, apiResponse.Data.UserId);
            Assert.Equal("Updated Name", apiResponse.Data.Name);
            _mockMediator.Verify(m => m.Send(It.Is<UpdateUserCommand>(cmd => cmd.Id == existingUserId && cmd.UserDto == updateDto), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistentId = 999;
            var updateDto = TestDataFactory.GetUpdateUserDto();
            _mockMediator.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((User?)null);

            // Act
            var result = await _controller.Update(nonExistentId, updateDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<User>>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("User not found", apiResponse.Message);
            Assert.Null(apiResponse.Data);
            _mockMediator.Verify(m => m.Send(It.Is<UpdateUserCommand>(cmd => cmd.Id == nonExistentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenUserIsDeleted()
        {
            // Arrange
            var existingUserId = 1;
            _mockMediator.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(existingUserId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal("User deleted successfully", apiResponse.Message);
            _mockMediator.Verify(m => m.Send(It.Is<DeleteUserCommand>(cmd => cmd.Id == existingUserId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistentId = 999;
            _mockMediator.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(nonExistentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(notFoundResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("User not found - please input valid User ID", apiResponse.Message);
            _mockMediator.Verify(m => m.Send(It.Is<DeleteUserCommand>(cmd => cmd.Id == nonExistentId), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}