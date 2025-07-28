using MediatR;
using ToDoApi.Application.Features.Users.Commands;
using ToDoApi.Application.Interfaces.IRepositories;
using Microsoft.Extensions.Logging;

namespace ToDoApi.Application.Features.Users.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DeleteUserCommandHandler> _logger;

        public DeleteUserCommandHandler(IUserRepository userRepository, ILogger<DeleteUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to delete User with ID: {UserId}", request.Id); 

            var result = await _userRepository.DeleteAsync(request.Id);

            if (result)
            {
                _logger.LogInformation("User with ID {UserId} deleted successfully.", request.Id); 
            }
            else
            {
                _logger.LogWarning("Failed to delete User with ID {UserId}. User not found or an error occurred.", request.Id); 
            }
            return result;
        }
    }
}