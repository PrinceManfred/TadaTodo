using FluentValidation;

namespace TadaTodo.Server.Dtos;

public class UpdateTodoListDtoValidator : AbstractValidator<UpdateTodoListDto>
{
    public UpdateTodoListDtoValidator(
        IValidator<CreateTodoItemDto> createTodoItemDtoValidator,
        IValidator<UpdateTodoItemDto> updateTodoItemDtoValidator)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty")
            .When(x => x.Name is not null);

        RuleForEach(x => x.NewItems).SetValidator(createTodoItemDtoValidator);

        RuleForEach(x => x.UpdatedItems).SetValidator(updateTodoItemDtoValidator);
    }
}