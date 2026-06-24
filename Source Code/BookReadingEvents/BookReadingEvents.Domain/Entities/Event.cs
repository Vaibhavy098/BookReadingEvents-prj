using BookReadingEvents.Domain.Enums;

namespace BookReadingEvents.Domain.Entities;

public class Event
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Location { get; set; } = string.Empty;

    public TimeSpan StartTime { get; set; }

    public int? DurationHours { get; set; }

    public EventType Type { get; set; } = EventType.Public;

    public string? Description { get; set; }

    public string? OtherDetails { get; set; }

    public string CreatorId { get; set; } = string.Empty;

    public ApplicationUser Creator { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Comment> Comments { get; set; }
        = new List<Comment>();

    public ICollection<Invitation> Invitations { get; set; }
        = new List<Invitation>();
}