using MediatR;
using ToDoApi.Application.Features.ToDos.Commands;
using ToDoApi.Application.Interfaces.IRepositories; 
using ToDoApi.Domain.Entities;

namespace ToDoApi.Application.Features.ToDos.Handlers
{
    public class CreateToDoCommandHandler : IRequestHandler<CreateToDoCommand, ToDo>
    {
        private readonly IToDoRepository _toDoRepository;

        public CreateToDoCommandHandler(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public async Task<ToDo> Handle(CreateToDoCommand request, CancellationToken cancellationToken)
        {
            var toDo = new ToDo
            {
                Title = request.ToDoDto.Title,
                Description = request.ToDoDto.Description,
                Status = false 
            };

            return await _toDoRepository.AddAsync(toDo);
        }
    }
}