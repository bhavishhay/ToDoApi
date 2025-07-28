using MediatR;
using ToDoApi.Application.Features.ToDos.Commands;
using ToDoApi.Application.Interfaces.IRepositories; 
using ToDoApi.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ToDoApi.Application.Features.ToDos.Handlers
{
    public class CreateToDoCommandHandler : IRequestHandler<CreateToDoCommand, ToDo>
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly ILogger<CreateToDoCommandHandler> _logger;

        public CreateToDoCommandHandler(IToDoRepository toDoRepository, ILogger<CreateToDoCommandHandler> logger)
        {
            _toDoRepository = toDoRepository;
            _logger = logger;
        }

        public async Task<ToDo> Handle(CreateToDoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating new ToDo with Title: {Title}", request.ToDoDto.Title);
            var toDo = new ToDo
            {
                Title = request.ToDoDto.Title,
                Description = request.ToDoDto.Description,
                Status = false 
            };
            var createdToDo = await _toDoRepository.AddAsync(toDo);
            _logger.LogInformation("ToDo created successfully with ID: {ToDoId}", createdToDo.Id);
            return createdToDo;
        }
    }
}