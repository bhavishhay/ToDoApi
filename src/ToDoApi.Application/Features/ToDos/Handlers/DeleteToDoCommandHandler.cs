using MediatR;
using ToDoApi.Application.Features.ToDos.Commands;
using ToDoApi.Application.Interfaces.IRepositories;
using Microsoft.Extensions.Logging;

namespace ToDoApi.Application.Features.ToDos.Handlers
{
    public class DeleteToDoCommandHandler : IRequestHandler<DeleteToDoCommand, bool>
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly ILogger<DeleteToDoCommandHandler> _logger;

        public DeleteToDoCommandHandler(IToDoRepository toDoRepository, ILogger<DeleteToDoCommandHandler> logger)
        {
            _toDoRepository = toDoRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteToDoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to delete ToDo with ID: {ToDoId}", request.Id);
            var result = await _toDoRepository.DeleteAsync(request.Id);

            if (result)
            {
                _logger.LogInformation("ToDo with ID {ToDoId} deleted successfully.", request.Id); 
            }
            else
            {
                _logger.LogWarning("Failed to delete ToDo with ID {ToDoId}. ToDo not found or an error occurred.", request.Id); 
            }
            return result;
        }
    }
}