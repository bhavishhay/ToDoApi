using MediatR;
using ToDoApi.Application.Features.ToDos.Commands;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Application.Features.ToDos.Handlers
{
    public class UpdateToDoCommandHandler : IRequestHandler<UpdateToDoCommand, ToDo?>
    {
        private readonly IToDoRepository _toDoRepository;

        public UpdateToDoCommandHandler(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public async Task<ToDo?> Handle(UpdateToDoCommand request, CancellationToken cancellationToken)
        {
            var toDo = await _toDoRepository.GetByIdAsync(request.Id);
            if (toDo == null) return null;

            toDo.Title = request.ToDoDto.Title;
            toDo.Description = request.ToDoDto.Description;
            toDo.Status = request.ToDoDto.Status;

            return await _toDoRepository.UpdateAsync(toDo);
        }
    }
}