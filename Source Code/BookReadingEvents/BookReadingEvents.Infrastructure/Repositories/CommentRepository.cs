using BookReadingEvents.Domain.Entities;
using BookReadingEvents.Domain.Interfaces.Repositories;
using BookReadingEvents.Infrastructure.Data;

namespace BookReadingEvents.Infrastructure.Repositories;

public class CommentRepository
    : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Comment comment)
    {
        await _context.Comments
            .AddAsync(comment);

        await _context.SaveChangesAsync();
    }
}
