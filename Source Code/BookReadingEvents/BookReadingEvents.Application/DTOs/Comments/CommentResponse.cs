namespace BookReadingEvents.Application.DTOs.Comments;
public class CommentResponse
{
    public int Id { get; set; }

    public string CommentText { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}