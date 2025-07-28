using MediatR;
using ToDoApi.Application.Features.ToDos.Commands;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ToDoApi.Application.Features.ToDos.Handlers
{
    public class UpdateToDoCommandHandler : IRequestHandler<UpdateToDoCommand, ToDo?>
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly ILogger<UpdateToDoCommandHandler> _logger;

        public UpdateToDoCommandHandler(IToDoRepository toDoRepository, ILogger<UpdateToDoCommandHandler> logger)
        {
            _toDoRepository = toDoRepository;
            _logger = logger;
        }

        public async Task<ToDo?> Handle(UpdateToDoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to update ToDo with ID: {ToDoId}", request.Id); 

            var toDo = await _toDoRepository.GetByIdAsync(request.Id);
            if (toDo == null)
            {
                _logger.LogWarning("ToDo with ID {ToDoId} not found for update.", request.Id); 
                return null;
            }

            _logger.LogInformation("Updating ToDo {ToDoId} from Title: '{OldTitle}' to '{NewTitle}', Status: '{OldStatus}' to '{NewStatus}'",
                                   toDo.Id, toDo.Title, request.ToDoDto.Title, toDo.Status, request.ToDoDto.Status);

            toDo.Title = request.ToDoDto.Title;
            toDo.Description = request.ToDoDto.Description;
            toDo.Status = request.ToDoDto.Status;

            var updatedToDo = await _toDoRepository.UpdateAsync(toDo);
            _logger.LogInformation("ToDo with ID {ToDoId} updated successfully.", request.Id); 
            return updatedToDo;
        }
    }
}