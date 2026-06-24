using BookReadingEvents.Domain.Entities;
using BookReadingEvents.Domain.Enums;
using BookReadingEvents.Domain.Interfaces.Repositories;
using BookReadingEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookReadingEvents.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _context;

    public EventRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Event eventEntity)
    {
        await _context.Events.AddAsync(eventEntity);

        await _context.SaveChangesAsync(); // generates SQL INSERT statement and saves to database
    }

    public async Task<Event?> GetByIdAsync(int id)
    {
        return await _context.Events
            .Include(e => e.Creator)
            .Include(e => e.Comments)
                .ThenInclude(c => c.User)
            .Include(e => e.Invitations)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Event>> GetPublicEventsAsync()
    {
        return await _context.Events
            .Where(e => e.Type == EventType.Public)
            .OrderBy(e => e.Date)
            .ToListAsync();
    }

    public async Task<List<Event>> GetEventsCreatedByUserAsync(
        string userId)
    {
        return await _context.Events
            .Where(e => e.CreatorId == userId)
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }

    public async Task<List<Event>> GetInvitedEventsAsync(
        string email)
    {
        return await _context.Events
            .Include(e => e.Invitations)
            .Where(e =>
                e.Invitations.Any(i =>
                    i.InvitedEmail == email))
            .ToListAsync();
    }

    public async Task UpdateAsync(Event eventEntity)
    {
        _context.Events.Update(eventEntity);

        await _context.SaveChangesAsync();
    }
}