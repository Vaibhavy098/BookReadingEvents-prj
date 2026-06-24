using BookReadingEvents.Domain.Enums;

namespace BookReadingEvents.Application.DTOs.Events;

public class CreateEventRequest
{
    public string Title { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Location { get; set; } = string.Empty;

    public TimeSpan StartTime { get; set; }

    public EventType Type { get; set; } = EventType.Public;

    public int? DurationHours { get; set; }

    public string? Description { get; set; }

    public string? OtherDetails { get; set; }

    public string? InviteEmails { get; set; }
}