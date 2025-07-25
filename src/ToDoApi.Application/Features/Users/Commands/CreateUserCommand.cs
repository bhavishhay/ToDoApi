using MediatR;
using ToDoApi.Domain.Entities;
using ToDoApi.Application.DTOs;

namespace ToDoApi.Application.Features.Users.Commands
{
    public record CreateUserCommand(CreateUserDto UserDto) : IRequest<User>;
}