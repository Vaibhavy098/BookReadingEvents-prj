using BookReadingEvents.Domain.Enums;
using BookReadingEvents.Application.DTOs.Comments;

namespace BookReadingEvents.Application.DTOs.Events;

public class EventDetailsResponse
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Location { get; set; } = string.Empty;

    public TimeSpan StartTime { get; set; }

    public int? DurationHours { get; set; }

    public EventType Type { get; set; }

    public string? Description { get; set; }

    public string? OtherDetails { get; set; }

    public string CreatorName { get; set; } = string.Empty;

    public int TotalInvitedUsers { get; set; }

    public List<CommentResponse> Comments { get; set; }
        = new();
}