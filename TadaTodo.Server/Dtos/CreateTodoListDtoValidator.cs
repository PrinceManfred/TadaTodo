using FluentValidation;

namespace TadaTodo.Server.Dtos;

public class CreateTodoListDtoValidator : AbstractValidator<CreateTodoListDto>
{
    public CreateTodoListDtoValidator(IValidator<CreateTodoItemDto> itemValidator)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name must be 255 characters or less.");

        RuleForEach(x => x.TodoItems).SetValidator(itemValidator);
    }
}