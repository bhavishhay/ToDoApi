using MediatR;
using ToDoApi.Domain.Entities;
using ToDoApi.Application.DTOs;

namespace ToDoApi.Application.Features.ToDos.Commands
{

    public record UpdateToDoCommand(int Id, UpdateToDoDto ToDoDto) : IRequest<ToDo?>;
}