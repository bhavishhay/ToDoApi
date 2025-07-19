using FluentValidation;
using ToDoApi.Application.DTOs;

namespace ToDoApi.Application.Validators
{
    public class UpdateToDoDtoValidator : AbstractValidator<UpdateToDoDto>
    {
        public UpdateToDoDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(5).WithMessage("Description be at least 5 characters long.")
                .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");

            RuleFor(x => x.Status)
            .NotNull().WithMessage("Status is required.")
                .Must(status => status == true || status == false)
                .WithMessage("Status must be a boolean value (true or false).");

        }
    }
}
