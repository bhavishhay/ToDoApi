using MediatR;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Application.Features.Users.Queries
{
    public record GetUserByIdQuery(int Id) : IRequest<User?>;
}