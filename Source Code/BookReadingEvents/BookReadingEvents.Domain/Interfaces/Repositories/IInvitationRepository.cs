using BookReadingEvents.Domain.Entities;

namespace BookReadingEvents.Domain.Interfaces.Repositories;

public interface IInvitationRepository
{
    Task AddRangeAsync(List<Invitation> invitations);
}