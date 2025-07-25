using MediatR;
namespace ToDoApi.Application.Features.Users.Commands
{
    public record DeleteUserCommand(int Id) : IRequest<bool>;
}