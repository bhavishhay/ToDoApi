using MediatR;
using ToDoApi.Application.Features.Users.Commands;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ToDoApi.Application.Features.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(IUserRepository userRepository, ILogger<CreateUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating new User with Name: {UserName} and Email: {UserEmail}", request.UserDto.Name, request.UserDto.Email);

            var user = new User
            {
                Name = request.UserDto.Name,
                Email = request.UserDto.Email,
                Address = request.UserDto.Address
            };

            var createdUser = await _userRepository.AddAsync(user);
            _logger.LogInformation("User created successfully with ID: {UserId}", createdUser.UserId); 
            return createdUser;
        }
    }
}