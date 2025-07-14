using FluentValidation;
using System.Data;
using ToDoApi.Models.DTOs;

namespace ToDoApi.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.");
        }
    }
}
