using Domain.ValueObjects;
using FluentValidation;

namespace Application.Users.Commands.Register;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x).NotEmpty();

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(UserName.MinLength)
            .MaximumLength(UserName.MaxLength);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(Email.MaxLength)
            .EmailAddress();

        RuleFor(x => x.FullName)
            .NotEmpty();
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(30);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(30);

        RuleFor(x => x)
            .Must(
                command =>
                    command.Password.Equals(command.ConfirmPassword))
            .WithMessage("Password and ConfirmPassword must be same.");
    }
}