using BookReadingEvents.Application.DTOs.Events;
using FluentValidation;

namespace BookReadingEvents.Application.Validators;

public class CreateEventRequestValidator
    : AbstractValidator<CreateEventRequest>
{
    public CreateEventRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Location)
            .NotEmpty()
            .MaximumLength(255);
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Type must be either 0 'Public' or 1 'Private'.");

        RuleFor(x => x.Description)
            .MaximumLength(50);

        RuleFor(x => x.OtherDetails)
            .MaximumLength(500);

        RuleFor(x => x.DurationHours)
            .LessThanOrEqualTo(4)
            .When(x => x.DurationHours.HasValue);
    }
}