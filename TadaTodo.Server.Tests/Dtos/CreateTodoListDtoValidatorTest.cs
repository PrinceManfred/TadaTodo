using FluentValidation.TestHelper;
using JetBrains.Annotations;
using TadaTodo.Server.Dtos;
using Xunit;

namespace TadaTodo.Server.Tests.Dtos;

[TestSubject(typeof(CreateTodoListDtoValidator))]
public class CreateTodoListDtoValidatorTest
{
    private readonly CreateTodoListDtoValidator _validator;

    public CreateTodoListDtoValidatorTest()
    {
        _validator = new CreateTodoListDtoValidator(new CreateTodoItemDtoValidator());
    }

    [Fact]
    public void Should_error_when_Name_is_null()
    {
        var list = new CreateTodoListDto(null!, null);

        var result = _validator.TestValidate(list);
        result.ShouldHaveValidationErrorFor(todoList => todoList.Name);
    }

    [Fact]
    public void Should_error_when_Name_is_empty()
    {
        var list = new CreateTodoListDto(string.Empty, null);

        var result = _validator.TestValidate(list);
        result.ShouldHaveValidationErrorFor(todoList => todoList.Name);
    }

    [Fact]
    public void Should_error_when_Name_is_over_255_characters_long()
    {
        var name = new string('x', 256);
        var list = new CreateTodoListDto(name, null);

        var result = _validator.TestValidate(list);
        result.ShouldHaveValidationErrorFor(todoList => todoList.Name);
    }

    [Fact]
    public void Should_not_error_when_Name_is_255_characters_long()
    {
        var name = new string('x', 255);
        var list = new CreateTodoListDto(name, null);

        var result = _validator.TestValidate(list);
        result.ShouldNotHaveValidationErrorFor(todoList => todoList.Name);
    }
}