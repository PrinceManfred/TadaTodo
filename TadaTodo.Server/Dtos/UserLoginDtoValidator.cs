using FluentValidation;

namespace TadaTodo.Server.Dtos;

public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginDtoValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 50).WithMessage("Username must be between 3 and 50 characters")
            .Matches(@"^[a-zA-Z0-9_-]+$")
            .WithMessage("Username can only contain alphanumeric characters, underscores, and hyphens");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required")
            .Length(6, 50).WithMessage("Password must be between 6 and 50 characters");
    }
}