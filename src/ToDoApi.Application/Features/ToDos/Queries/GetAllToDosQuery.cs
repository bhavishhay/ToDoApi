using MediatR;
using ToDoApi.Domain.Entities;
using ToDoApi.Application.QueryParameters;

namespace ToDoApi.Application.Features.ToDos.Queries
{
    public record GetAllToDosQuery(ToDoQuaryFilterSortingParameters QueryParameters) : IRequest<IEnumerable<ToDo>>;
}