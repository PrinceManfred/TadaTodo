using FluentValidation;
using TadaTodo.Server.Dtos;

public class UpdateTodoItemDtoValidator : AbstractValidator<UpdateTodoItemDto>
{
    public UpdateTodoItemDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0.");

        RuleFor(x => x.Value)
            .NotNull().WithMessage("Value cannot be null.")
            .MaximumLength(500)
            .WithMessage("Value must be 500 characters or less.");
    }
}