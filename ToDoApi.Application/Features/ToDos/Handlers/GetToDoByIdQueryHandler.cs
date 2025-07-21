using MediatR;
using ToDoApi.Application.Features.ToDos.Queries;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Application.Features.ToDos.Handlers
{
    public class GetToDoByIdQueryHandler : IRequestHandler<GetToDoByIdQuery, ToDo?>
    {
        private readonly IToDoRepository _toDoRepository;

        public GetToDoByIdQueryHandler(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public async Task<ToDo?> Handle(GetToDoByIdQuery request, CancellationToken cancellationToken)
        {
            return await _toDoRepository.GetByIdAsync(request.Id);
        }
    }
}