using MediatR;
using ToDoApi.Application.Features.ToDos.Queries;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Application.Features.ToDos.Handlers
{
    public class GetAllToDosQueryHandler : IRequestHandler<GetAllToDosQuery, IEnumerable<ToDo>>
    {
        private readonly IToDoRepository _toDoRepository;

        public GetAllToDosQueryHandler(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public async Task<IEnumerable<ToDo>> Handle(GetAllToDosQuery request, CancellationToken cancellationToken)
        {
            return await _toDoRepository.GetAllAsync(request.QueryParameters);
        }
    }
}