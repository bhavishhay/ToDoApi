using MediatR;
using ToDoApi.Application.Features.Users.Queries;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ToDoApi.Application.Features.Users.Handlers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;

        public GetAllUsersQueryHandler(IUserRepository userRepository, ILogger<GetAllUsersQueryHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all Users with PageNumber: {PageNumber}, PageSize: {PageSize}, SortBy: {SortBy}, SortDescending: {SortDescending}",
                                               request.QueryParameters.PageNumber, request.QueryParameters.PageSize,
                                               request.QueryParameters.SortBy, request.QueryParameters.SortDescending); 
            var users = await _userRepository.GetAllAsync(request.QueryParameters);

            if (users == null || !users.Any())
            {
                _logger.LogInformation("No Users found matching the query parameters.");
            }
            else
            {
                _logger.LogInformation("Successfully fetched {UserCount} Users.", users.Count()); 
            }
            return users;
        }
    }
}