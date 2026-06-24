using BookReadingEvents.Domain.Entities;

namespace BookReadingEvents.Domain.Interfaces.Repositories;

public interface ICommentRepository
{
    Task AddAsync(Comment comment);
}