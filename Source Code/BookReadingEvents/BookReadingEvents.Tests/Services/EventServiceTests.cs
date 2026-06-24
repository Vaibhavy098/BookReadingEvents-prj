using BookReadingEvents.Application.DTOs.Events;
using BookReadingEvents.Application.Services;
using BookReadingEvents.Domain.Entities;
using BookReadingEvents.Domain.Interfaces.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace BookReadingEvents.Tests.Services;

public class EventServiceTests
{
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly Mock<IInvitationRepository> _invitationRepositoryMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;

    private readonly EventService _eventService;

    public EventServiceTests()
    {
        _eventRepositoryMock =
            new Mock<IEventRepository>();

        _invitationRepositoryMock =
            new Mock<IInvitationRepository>();

        var store =
            new Mock<IUserStore<ApplicationUser>>();

        _userManagerMock =
            new Mock<UserManager<ApplicationUser>>(
                store.Object,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!);

        _eventService =
            new EventService(
                _eventRepositoryMock.Object,
                _invitationRepositoryMock.Object,
                _userManagerMock.Object);
    }


    [Fact]
    public async Task CreateEvent_Should_Create_Event()
    {
        var request =
            new CreateEventRequest
            {
                Title = "Test Event",
                Date = DateTime.Today,
                Location = "Delhi",
                StartTime = new TimeSpan(10, 0, 0)
            };

        await _eventService.CreateEventAsync(
            request,
            "user1");

        _eventRepositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Event>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateEvent_Should_Create_Invitations_When_Emails_Are_Provided()
    {
        var request =
            new CreateEventRequest
            {
                Title = "Test Event",
                Date = DateTime.Today,
                Location = "Delhi",
                StartTime = new TimeSpan(10, 0, 0),
                InviteEmails =
                    "test1@gmail.com,test2@gmail.com"
            };

        await _eventService.CreateEventAsync(
            request,
            "user1");

        _invitationRepositoryMock.Verify(
            x => x.AddRangeAsync(
                It.IsAny<List<Invitation>>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateEvent_Should_Not_Create_Invitations_When_Email_List_Is_Empty()
    {
        var request =
            new CreateEventRequest
            {
                Title = "Test Event",
                Date = DateTime.Today,
                Location = "Delhi",
                StartTime = new TimeSpan(10, 0, 0)
            };

        await _eventService.CreateEventAsync(
            request,
            "user1");

        _invitationRepositoryMock.Verify(
            x => x.AddRangeAsync(
                It.IsAny<List<Invitation>>()),
            Times.Never);
    }


    [Fact]
    public async Task GetPublicEvents_Should_Return_Public_Events()
    {
        var events =
            new List<Event>
            {
            new()
            {
                Id = 1,
                Title = "Event 1"
            },
            new()
            {
                Id = 2,
                Title = "Event 2"
            }
            };

        _eventRepositoryMock
            .Setup(x =>
                x.GetPublicEventsAsync())
            .ReturnsAsync(events);

        var result =
            await _eventService
                .GetPublicEventsAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetMyEvents_Should_Return_User_Events()
    {
        var events =
            new List<Event>
            {
            new()
            {
                Id = 1,
                CreatorId = "user1"
            },
            new()
            {
                Id = 2,
                CreatorId = "user1"
            }
            };

        _eventRepositoryMock
            .Setup(x =>
                x.GetEventsCreatedByUserAsync(
                    "user1"))
            .ReturnsAsync(events);

        var result =
            await _eventService
                .GetMyEventsAsync(
                    "user1");

        result.Should().HaveCount(2);
    }
}