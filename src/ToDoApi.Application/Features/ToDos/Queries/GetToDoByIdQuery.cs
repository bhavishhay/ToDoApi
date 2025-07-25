using MediatR;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Application.Features.ToDos.Queries
{
    public record GetToDoByIdQuery(int Id) : IRequest<ToDo?>;
}