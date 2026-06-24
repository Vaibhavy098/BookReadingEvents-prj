using BookReadingEvents.Domain.Entities;

namespace BookReadingEvents.Domain.Interfaces.Repositories;

public interface IEventRepository
{
    Task AddAsync(Event eventEntity);

    Task<Event?> GetByIdAsync(int id);

    Task<List<Event>> GetPublicEventsAsync();

    Task<List<Event>> GetEventsCreatedByUserAsync(string userId);

    Task<List<Event>> GetInvitedEventsAsync(string email);

    Task UpdateAsync(Event eventEntity);
}