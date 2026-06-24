using BookReadingEvents.Application.DTOs.Events;

namespace BookReadingEvents.Application.Interfaces;

public interface IEventService
{
    Task CreateEventAsync(CreateEventRequest request, string userId);

    Task<List<EventResponse>> GetPublicEventsAsync();

    Task<EventDetailsResponse> GetEventDetailsAsync(int eventId, string? userId, string? email);

    Task<List<EventResponse>> GetMyEventsAsync(string userId);

    Task<List<EventResponse>> GetInvitedEventsAsync(string email);

    Task UpdateEventAsync(int eventId, UpdateEventRequest request, string userId);
}