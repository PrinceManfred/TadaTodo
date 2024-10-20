using FluentValidation.TestHelper;
using JetBrains.Annotations;
using TadaTodo.Server.Dtos;
using Xunit;

namespace TadaTodo.Server.Tests.Dtos;

[TestSubject(typeof(UpdateTodoItemDtoValidator))]
public class UpdateTodoItemDtoValidatorTest
{
    private readonly UpdateTodoItemDtoValidator _validator = new();

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_have_error_when_Id_is_less_than_or_equal_to_zero(int id)
    {
        var item = new UpdateTodoItemDto(id, string.Empty, false);

        var result = _validator.TestValidate(item);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_not_have_error_when_Id_is_greater_than_zero()
    {
        var item = new UpdateTodoItemDto(1, string.Empty, false);

        var result = _validator.TestValidate(item);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_have_error_when_Value_is_null()
    {
        var item = new UpdateTodoItemDto(1, null!, false);

        var result = _validator.TestValidate(item);
        result.ShouldHaveValidationErrorFor(x => x.Value);
    }

    [Fact]
    public void Should_have_error_when_Value_length_is_over_500_characters_long()
    {
        var item = new UpdateTodoItemDto(1, new string('x', 501), false);

        var result = _validator.TestValidate(item);
        result.ShouldHaveValidationErrorFor(x => x.Value);
    }

    [Fact]
    public void Should_not_have_error_when_Value_is_empty()
    {
        var item = new UpdateTodoItemDto(1, string.Empty, false);

        var result = _validator.TestValidate(item);
        result.ShouldNotHaveValidationErrorFor(x => x.Value);
    }

    [Fact]
    public void Should_not_have_error_when_Value_length_is_500_characters_long()
    {
        var item = new UpdateTodoItemDto(1, new string('x', 500), false);

        var result = _validator.TestValidate(item);
        result.ShouldNotHaveValidationErrorFor(x => x.Value);
    }
}