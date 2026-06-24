using BookReadingEvents.Application.DTOs.Comments;
using BookReadingEvents.Application.Exceptions;
using BookReadingEvents.Application.Interfaces;
using BookReadingEvents.Domain.Entities;
using BookReadingEvents.Domain.Interfaces.Repositories;

namespace BookReadingEvents.Application.Services;

public class CommentService
    : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IEventRepository _eventRepository;

    public CommentService(ICommentRepository commentRepository, IEventRepository eventRepository)
    {
        _commentRepository = commentRepository;
        _eventRepository = eventRepository;
    }

    public async Task AddCommentAsync(
        int eventId,
        string userId,
        AddCommentRequest request)
    {
        var eventEntity =
    await _eventRepository.GetByIdAsync(eventId);

        if (eventEntity == null)
        {
            throw new NotFoundException(
                $"Event with ID {eventId} not found.");
        }
        var comment =
            new Comment
            {
                EventId = eventId,
                UserId = userId,
                CommentText = request.CommentText,
                CreatedAt = DateTime.UtcNow
            };

        await _commentRepository
            .AddAsync(comment);
    }
}