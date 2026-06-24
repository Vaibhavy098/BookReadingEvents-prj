using BookReadingEvents.Application.DTOs.Comments;
using BookReadingEvents.Application.DTOs.Events;
using BookReadingEvents.Application.Interfaces;
using BookReadingEvents.Domain.Entities;
using BookReadingEvents.Domain.Enums;
using BookReadingEvents.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using BookReadingEvents.Application.Exceptions;

namespace BookReadingEvents.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IInvitationRepository _invitationRepository;

        public EventService(IEventRepository eventRepository, IInvitationRepository invitationRepository, UserManager<ApplicationUser> userManager)
        {
            _eventRepository = eventRepository;
            _invitationRepository = invitationRepository;
            _userManager = userManager;
        }
        public async Task CreateEventAsync(CreateEventRequest request, string userId)
        {
            var eventEntity = new Event
            {
                Title = request.Title,
                Date = request.Date,
                Location = request.Location,
                StartTime = request.StartTime,
                DurationHours = request.DurationHours,
                Type = request.Type,
                Description = request.Description,
                OtherDetails = request.OtherDetails,
                CreatorId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _eventRepository.AddAsync(eventEntity);

            if (!string.IsNullOrWhiteSpace(request.InviteEmails))
            {
                var emails =
                    request.InviteEmails
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(e => e.Trim())
                        .Distinct()
                        .ToList();

                var invitations =
                    emails.Select(email =>
                        new Invitation
                        {
                            EventId = eventEntity.Id,
                            InvitedEmail = email,
                            InvitedAt = DateTime.UtcNow
                        })
                    .ToList();

                await _invitationRepository
                    .AddRangeAsync(invitations);
            }
        }

        public async Task<EventDetailsResponse> GetEventDetailsAsync(int eventId, string? userId, string? email)
        {
            var eventEntity =
                await _eventRepository.GetByIdAsync(eventId);

            if (eventEntity == null)
            {
                throw new NotFoundException("Event not found.");
            }

            if (eventEntity.Type == EventType.Private)
            {
                bool isCreator =
                    eventEntity.CreatorId == userId;

                bool isInvited =
                    eventEntity.Invitations.Any(i =>
                        i.InvitedEmail == email);

                bool isAdmin = false;

                if (!string.IsNullOrEmpty(userId))
                {
                    var user =
                        await _userManager.FindByIdAsync(userId);

                    if (user != null)
                    {
                        isAdmin =
                            await _userManager
                                .IsInRoleAsync(user, "Admin");
                    }
                }

                if (!isCreator && !isInvited && !isAdmin)
                {
                    throw new ForbiddenException(
                        "You are not allowed to view this event.");
                }
            }

            return new EventDetailsResponse
            {
                Id = eventEntity.Id,
                Title = eventEntity.Title,
                Date = eventEntity.Date,
                Location = eventEntity.Location,
                StartTime = eventEntity.StartTime,
                DurationHours = eventEntity.DurationHours,
                Type = eventEntity.Type,
                Description = eventEntity.Description,
                OtherDetails = eventEntity.OtherDetails,

                CreatorName =
                    eventEntity.Creator.FullName,

                TotalInvitedUsers =
                    eventEntity.Invitations.Count,

                Comments =
                    eventEntity.Comments
                        .OrderBy(c => c.CreatedAt)
                        .Select(c => new CommentResponse
                        {
                            Id = c.Id,
                            CommentText = c.CommentText,
                            UserName = c.User.FullName,
                            CreatedAt = c.CreatedAt
                        })
                        .ToList()
            };
        }

        public async Task<List<EventResponse>> GetInvitedEventsAsync(string email)
        {
            var events =
                await _eventRepository
                    .GetInvitedEventsAsync(email);

            return events.Select(e => new EventResponse
            {
                Id = e.Id,
                Title = e.Title,
                Date = e.Date,
                Location = e.Location,
                Type = e.Type
            }).ToList();
        }

        public async Task<List<EventResponse>> GetMyEventsAsync(string userId)
        {
            var events =
                await _eventRepository
                    .GetEventsCreatedByUserAsync(userId);

            return events.Select(e => new EventResponse
            {
                Id = e.Id,
                Title = e.Title,
                Date = e.Date,
                Location = e.Location,
                Type = e.Type
            }).ToList();
        }
        

        public async Task<List<EventResponse>> GetPublicEventsAsync()
        {
            var events =
                await _eventRepository.GetPublicEventsAsync();

            return events.Select(e => new EventResponse
            {
                Id = e.Id,
                Title = e.Title,
                Date = e.Date,
                Location = e.Location,
                Type = e.Type
            }).ToList();
        }

        public async Task UpdateEventAsync(int eventId, UpdateEventRequest request, string userId)
        {
            var eventEntity =
                await _eventRepository.GetByIdAsync(eventId);

            if (eventEntity == null)
            {
                throw new NotFoundException(
                    "Event not found.");
            }

            var user =
                await _userManager.FindByIdAsync(userId);

            bool isAdmin = false;

            if (user != null)
            {
                isAdmin =
                    await _userManager
                        .IsInRoleAsync(user, "Admin");
            }

            bool isCreator =
                eventEntity.CreatorId == userId;

            if (!isCreator && !isAdmin)
            {
                throw new ForbiddenException(
                    "Only creator or admin can edit event.");
            }

            var eventStartDateTime =
                eventEntity.Date.Date +
                eventEntity.StartTime;

            if (eventStartDateTime < DateTime.Now)
            {
                throw new BusinessRuleException(
                    "Past events cannot be edited.");
            }

            eventEntity.Title = request.Title;
            eventEntity.Date = request.Date;
            eventEntity.Location = request.Location;
            eventEntity.StartTime = request.StartTime;
            eventEntity.DurationHours = request.DurationHours;
            eventEntity.Type = request.Type;
            eventEntity.Description = request.Description;
            eventEntity.OtherDetails = request.OtherDetails;

            await _eventRepository.UpdateAsync(eventEntity);
        }
    }
}
