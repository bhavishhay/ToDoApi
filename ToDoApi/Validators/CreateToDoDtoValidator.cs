using FluentValidation;
using System.Data;
using ToDoApi.Models.DTOs;

namespace ToDoApi.Validators
{
    public class CreateToDoDtoValidator : AbstractValidator<CreateToDoDto>
    {
        public CreateToDoDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(5).WithMessage("Description be at least 5 characters long.")
                .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");
        }
    }
}
