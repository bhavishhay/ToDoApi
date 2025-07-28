using MediatR;
using ToDoApi.Application.Features.Users.Queries;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ToDoApi.Application.Features.Users.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User?>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetUserByIdQueryHandler> _logger;

        public GetUserByIdQueryHandler(IUserRepository userRepository, ILogger<GetUserByIdQueryHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching User by ID: {UserId}", request.Id); 
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", request.Id); 
            }
            else
            {
                _logger.LogInformation("User with ID {UserId} found.", request.Id); 
            }
            return user;
        }
    }
}