using BookReadingEvents.Application.DTOs.Requests;
using FluentValidation;

namespace BookReadingEvents.Application.Validators;

public class RegisterRequestValidator
    : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
            .WithMessage("Please enter a valid email address."); // if i dont write this message here the default message will be "Email is not a valid email address." from fluentvalidation.

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(5)
            .Matches(@"[A-Z]")
            .WithMessage(
                "Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]")
            .WithMessage(
                "Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]")
                .WithMessage("Password must contain a number")
            .Matches(@"[^a-zA-Z0-9]")
                .WithMessage(
                "Password must contain at least one special character.");
    }
}