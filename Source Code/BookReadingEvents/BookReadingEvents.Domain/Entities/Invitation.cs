namespace BookReadingEvents.Domain.Entities;

public class Invitation 
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public Event Event { get; set; } = null!;

    public string InvitedEmail { get; set; } = string.Empty;

    public DateTime InvitedAt { get; set; }
}