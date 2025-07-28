using MediatR;
using ToDoApi.Application.Features.Users.Commands;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ToDoApi.Application.Features.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, User?>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdateUserCommandHandler> _logger;

        public UpdateUserCommandHandler(IUserRepository userRepository, ILogger<UpdateUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<User?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to update User with ID: {UserId}", request.Id); // Log request

            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for update.", request.Id); // Not found log
                return null;
            }

            _logger.LogInformation("Updating User {UserId} from Name: '{OldName}' to '{NewName}', Email: '{OldEmail}' to '{NewEmail}'",
                                   user.UserId, user.Name, request.UserDto.Name, user.Email, request.UserDto.Email); // Log changes

            user.Name = request.UserDto.Name;
            user.Email = request.UserDto.Email;
            user.Address = request.UserDto.Address;

            var updatedUser = await _userRepository.UpdateAsync(user);
            _logger.LogInformation("User with ID {UserId} updated successfully.", request.Id); // Success log
            return updatedUser;
        }
    }
}