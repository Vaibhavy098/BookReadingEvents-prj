using BookReadingEvents.Application.DTOs.Comments;

namespace BookReadingEvents.Application.Interfaces;

public interface ICommentService
{
    Task AddCommentAsync(
        int eventId,
        string userId,
        AddCommentRequest request);
}