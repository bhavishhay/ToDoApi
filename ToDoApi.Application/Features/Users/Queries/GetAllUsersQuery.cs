using MediatR;
using ToDoApi.Domain.Entities;
using ToDoApi.Application.QueryParameters;

namespace ToDoApi.Application.Features.Users.Queries
{
    public record GetAllUsersQuery(UserQuaryFilterSortingParameters QueryParameters) : IRequest<IEnumerable<User>>;
}