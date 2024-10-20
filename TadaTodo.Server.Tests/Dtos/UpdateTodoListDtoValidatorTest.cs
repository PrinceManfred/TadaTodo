using FluentValidation.TestHelper;
using JetBrains.Annotations;
using TadaTodo.Server.Dtos;
using Xunit;

namespace TadaTodo.Server.Tests.Dtos;

[TestSubject(typeof(UpdateTodoListDtoValidator))]
public class UpdateTodoListDtoValidatorTest
{
    private readonly UpdateTodoListDtoValidator _validator;

    public UpdateTodoListDtoValidatorTest()
    {
        _validator =
            new UpdateTodoListDtoValidator(new CreateTodoItemDtoValidator(), new UpdateTodoItemDtoValidator());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_have_error_when_Id_is_less_than_or_equal_to_zero(int id)
    {
        var list = new UpdateTodoListDto(id, "Todo", null, null, null);

        var result = _validator.TestValidate(list);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_not_have_error_when_Id_is_greater_than_zero()
    {
        var list = new UpdateTodoListDto(1, "Todo", null, null, null);

        var result = _validator.TestValidate(list);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_have_error_when_Name_is_empty_and_not_null()
    {
        var list = new UpdateTodoListDto(1, string.Empty, null, null, null);

        var result = _validator.TestValidate(list);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_not_have_error_when_Name_is_not_empty()
    {
        var list = new UpdateTodoListDto(1, "Todo", null, null, null);

        var result = _validator.TestValidate(list);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}