using MediatR;
using ToDoApi.Domain.Entities;
using ToDoApi.Application.DTOs;

namespace ToDoApi.Application.Features.Users.Commands
{
    public record UpdateUserCommand(int Id, UpdateUserDto UserDto) : IRequest<User?>;
}