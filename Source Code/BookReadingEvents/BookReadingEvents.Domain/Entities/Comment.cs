namespace BookReadingEvents.Domain.Entities;

public class Comment
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public Event Event { get; set; } = null!; // Navigation property to the associated Event

    public string UserId { get; set; } = string.Empty; // who wrote this comment (Notice that we are using string for UserId because IdentityUser's Id is of type string)

    public ApplicationUser User { get; set; } = null!; // Navigation property to the associated User

    public string? CommentText { get; set; }

    public DateTime CreatedAt { get; set; }
}