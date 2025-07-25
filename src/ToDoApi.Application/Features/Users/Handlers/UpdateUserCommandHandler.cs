using MediatR;
using ToDoApi.Application.Features.Users.Commands;
using ToDoApi.Application.Interfaces.IRepositories;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Application.Features.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, User?>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null) return null;

            user.Name = request.UserDto.Name;
            user.Email = request.UserDto.Email;
            user.Address = request.UserDto.Address;

            return await _userRepository.UpdateAsync(user);
        }
    }
}