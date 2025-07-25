using MediatR;
using ToDoApi.Application.Features.ToDos.Commands;
using ToDoApi.Application.Interfaces.IRepositories;

namespace ToDoApi.Application.Features.ToDos.Handlers
{
    public class DeleteToDoCommandHandler : IRequestHandler<DeleteToDoCommand, bool>
    {
        private readonly IToDoRepository _toDoRepository;

        public DeleteToDoCommandHandler(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public async Task<bool> Handle(DeleteToDoCommand request, CancellationToken cancellationToken)
        {
            return await _toDoRepository.DeleteAsync(request.Id);
        }
    }
}