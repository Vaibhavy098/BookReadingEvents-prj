using BookReadingEvents.Application.DTOs.Events;
using FluentValidation;

namespace BookReadingEvents.Application.Validators;

public class UpdateEventRequestValidator
    : AbstractValidator<UpdateEventRequest>
{
    public UpdateEventRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Location)
            .NotEmpty();

        RuleFor(x => x.DurationHours)
            .LessThanOrEqualTo(4);

        RuleFor(x => x.Description)
            .MaximumLength(50);

        RuleFor(x => x.OtherDetails)
            .MaximumLength(500);
    }
}