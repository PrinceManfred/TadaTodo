using FluentValidation;

namespace TadaTodo.Server.Dtos;

public class CreateTodoItemDtoValidator : AbstractValidator<CreateTodoItemDto>
{
    public CreateTodoItemDtoValidator()
    {
        RuleFor(x => x.Value)
            .NotNull().WithMessage("Value cannot be null.")
            .MaximumLength(500).WithMessage("Value must be 500 characters or less.");
    }
}