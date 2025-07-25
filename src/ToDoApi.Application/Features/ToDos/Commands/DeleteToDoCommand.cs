using MediatR;

namespace ToDoApi.Application.Features.ToDos.Commands
{
    public record DeleteToDoCommand(int Id) : IRequest<bool>;
}