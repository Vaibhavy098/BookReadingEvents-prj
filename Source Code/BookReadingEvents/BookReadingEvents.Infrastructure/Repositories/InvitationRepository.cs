using BookReadingEvents.Domain.Entities;
using BookReadingEvents.Domain.Interfaces.Repositories;
using BookReadingEvents.Infrastructure.Data;

namespace BookReadingEvents.Infrastructure.Repositories;

public class InvitationRepository
    : IInvitationRepository
{
    private readonly ApplicationDbContext _context;

    public InvitationRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddRangeAsync(
        List<Invitation> invitations)
    {
        await _context.Invitations
            .AddRangeAsync(invitations);

        await _context.SaveChangesAsync();
    }
}