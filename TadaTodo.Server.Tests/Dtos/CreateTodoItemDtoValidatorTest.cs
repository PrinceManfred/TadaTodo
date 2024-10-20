using FluentValidation.TestHelper;
using JetBrains.Annotations;
using TadaTodo.Server.Dtos;
using Xunit;

namespace TadaTodo.Server.Tests.Dtos;

[TestSubject(typeof(CreateTodoItemDtoValidator))]
public class CreateTodoItemDtoValidatorTest
{
    private readonly CreateTodoItemDtoValidator _validator = new();

    [Fact]
    public void Should_have_error_when_Value_is_null()
    {
        var item = new CreateTodoItemDto(null!, false);

        var result = _validator.TestValidate(item);
        result.ShouldHaveValidationErrorFor(x => x.Value);
    }

    [Fact]
    public void Should_not_have_error_when_Value_is_specified()
    {
        var item = new CreateTodoItemDto("Test Value", false);

        var result = _validator.TestValidate(item);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_have_error_when_Value_is_over_500_characters_long()
    {
        var item = new CreateTodoItemDto(new string('x', 501), false);

        var result = _validator.TestValidate(item);
        result.ShouldHaveValidationErrorFor(x => x.Value);
    }

    [Fact]
    public void Should_not_have_error_when_Value_is_500_characters_long()
    {
        var item = new CreateTodoItemDto(new string('x', 500), false);

        var result = _validator.TestValidate(item);
        result.ShouldNotHaveValidationErrorFor(x => x.Value);
    }
}