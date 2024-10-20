using FluentValidation.TestHelper;
using JetBrains.Annotations;
using TadaTodo.Server.Dtos;
using Xunit;

namespace TadaTodo.Server.Tests.Dtos;

[TestSubject(typeof(UserLoginDtoValidator))]
public class UserLoginDtoValidatorTest
{
    private readonly UserLoginDtoValidator _validator = new();

    [Fact]
    public void Should_have_error_when_Username_is_null()
    {
        var user = new UserLoginDto(null!, "testPassword");

        var result = _validator.TestValidate(user);
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Should_have_error_when_Username_is_empty()
    {
        var user = new UserLoginDto(string.Empty, "testPassword");

        var result = _validator.TestValidate(user);
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Should_have_error_when_Username_is_less_than_3_characters()
    {
        var user = new UserLoginDto("xy", "testPassword");

        var result = _validator.TestValidate(user);
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Should_have_error_when_Username_is_more_than_50_characters()
    {
        var longUsername = new string('x', 51);
        var user = new UserLoginDto(longUsername, "testPassword");

        var result = _validator.TestValidate(user);
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Should_have_error_when_Username_contains_non_alphanumeric_character()
    {
        var user = new UserLoginDto("abc#", "testPassword");

        var result = _validator.TestValidate(user);
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Should_not_have_error_when_Username_is_50_characters()
    {
        var username = new string('x', 50);
        var user = new UserLoginDto(username, "testPassword");

        var result = _validator.TestValidate(user);
        result.ShouldNotHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Should_have_error_when_Password_is_null()
    {
        var user = new UserLoginDto("testUsername", null!);

        var result = _validator.TestValidate(user);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_have_error_when_Password_is_empty()
    {
        var user = new UserLoginDto("testUsername", string.Empty);

        var result = _validator.TestValidate(user);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_have_error_when_Password_is_less_than_6_characters()
    {
        var user = new UserLoginDto("testUsername", "12345");

        var result = _validator.TestValidate(user);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("123456")]
    [InlineData("1234567")]
    public void Should_not_have_error_when_Password_is_greater_than_or_equal_to_6_characters(string password)
    {
        var user = new UserLoginDto("testUsername123", password);

        var result = _validator.TestValidate(user);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}