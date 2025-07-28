using MediatR;
using ToDoApi.Application.Features.ToDos.Queries;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ToDoApi.Application.Features.ToDos.Handlers
{
    public class GetAllToDosQueryHandler : IRequestHandler<GetAllToDosQuery, IEnumerable<ToDo>>
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly ILogger<GetAllToDosQueryHandler> _logger;

        public GetAllToDosQueryHandler(IToDoRepository toDoRepository, ILogger<GetAllToDosQueryHandler> logger)
        {
            _toDoRepository = toDoRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ToDo>> Handle(GetAllToDosQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all ToDos with PageNumber: {PageNumber}, PageSize: {PageSize}, SortBy: {SortBy}, SortDescending: {SortDescending}",
                                               request.QueryParameters.PageNumber, request.QueryParameters.PageSize,
                                               request.QueryParameters.SortBy, request.QueryParameters.SortDescending);

            var toDos = await _toDoRepository.GetAllAsync(request.QueryParameters);

            if (toDos == null || !toDos.Any())
            {
                _logger.LogInformation("No ToDos found matching the query parameters."); 
            }
            else
            {
                _logger.LogInformation("Successfully fetched {ToDoCount} ToDos.", toDos.Count()); 
            }
            return toDos;
        }
    }
}