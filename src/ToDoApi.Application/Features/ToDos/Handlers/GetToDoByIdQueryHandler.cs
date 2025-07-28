using MediatR;
using ToDoApi.Application.Features.ToDos.Queries;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ToDoApi.Application.Features.ToDos.Handlers
{
    public class GetToDoByIdQueryHandler : IRequestHandler<GetToDoByIdQuery, ToDo?>
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly ILogger<GetToDoByIdQueryHandler> _logger;

        public GetToDoByIdQueryHandler(IToDoRepository toDoRepository, ILogger<GetToDoByIdQueryHandler> logger)
        {
            _toDoRepository = toDoRepository;
            _logger = logger;
        }

        public async Task<ToDo?> Handle(GetToDoByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching ToDo by ID: {ToDoId}", request.Id); 

            var toDo = await _toDoRepository.GetByIdAsync(request.Id);

            if (toDo == null)
            {
                _logger.LogWarning("ToDo with ID {ToDoId} not found.", request.Id);
            }
            else
            {
                _logger.LogInformation("ToDo with ID {ToDoId} found.", request.Id); 
            }
            return toDo;
        }
    }
}