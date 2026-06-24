using BookReadingEvents.Domain.Entities;

namespace BookReadingEvents.Application.Interfaces;

public interface IJwtService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
}