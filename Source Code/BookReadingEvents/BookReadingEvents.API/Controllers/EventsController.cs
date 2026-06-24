using BookReadingEvents.Application.DTOs.Comments;
using BookReadingEvents.Application.DTOs.Events;
using BookReadingEvents.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookReadingEvents.API.Controllers;

/// <summary>
/// Handles book reading event management operations.
/// </summary>
/// <remarks>
/// Supports event creation, retrieval, updates,
/// invitations, and comments.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ICommentService _commentService;

    public EventsController(IEventService eventService, ICommentService commentService)
    {
        _eventService = eventService;
        _commentService = commentService;
    }


    /// <summary>
    /// Creates a new book reading event.
    /// </summary>
    /// <remarks>
    /// Only authenticated users can create events.
    /// Supports public and private events.
    /// Invitation emails can be provided.
    /// </remarks>
    /// <response code="200">Event created successfully.</response>
    /// <response code="401">Authentication required.</response>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateEvent(
        CreateEventRequest request)
    {
        var userId =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        await _eventService.CreateEventAsync(
            request,
            userId!);

        return Ok("Event created successfully.");
    }
    /// <summary>
    /// Retrieves all public book reading events.
    /// </summary>
    /// <remarks>
    /// Accessible to both anonymous and authenticated users.
    /// Returns public events regardless of whether they are past or upcoming.
    /// </remarks>
    /// <response code="200">List of public events returned successfully.</response>
    [HttpGet("public")]
    public async Task<IActionResult> GetPublicEvents()
    {
        var events =
            await _eventService.GetPublicEventsAsync();

        return Ok(events);
    }
    /// <summary>
    /// Retrieves events created by the currently logged-in user.
    /// </summary>
    /// <remarks>
    /// Returns all events created by the authenticated user.
    /// Events are sorted by newest start date first.
    /// </remarks>
    /// <response code="200">User events returned successfully.</response>
    /// <response code="401">Authentication required.</response>
    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyEvents()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var events = await _eventService.GetMyEventsAsync(userId);

        return Ok(events);
    }
    /// <summary>
    /// Retrieves detailed information about a specific event.
    /// </summary>
    /// <remarks>
    /// Public events can be accessed by anyone.
    /// Private events can only be accessed by:
    /// - Event Creator
    /// - Invited Users
    /// - Administrators
    ///
    /// Response includes:
    /// - Event details
    /// - Total invited users count
    /// - Event comments sorted by oldest first
    /// </remarks>
    /// <param name="id">Event identifier.</param>
    /// <response code="200">Event details returned successfully.</response>
    /// <response code="403">User is not authorized to view this private event.</response>
    /// <response code="404">Event not found.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventDetails(
    int id)
    {
        var userId =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        var email =
            User.FindFirstValue(
                ClaimTypes.Email);

        var eventDetails =
            await _eventService
                .GetEventDetailsAsync(
                    id,
                    userId,
                    email);

        return Ok(eventDetails);
    }
    /// <summary>
    /// Updates an existing event.
    /// </summary>
    /// <remarks>
    /// Only the event creator or an administrator can update an event.
    /// Events whose start date and time have already passed cannot be modified.
    /// </remarks>
    /// <param name="id">Event identifier.</param>
    /// <param name="request">Updated event information.</param>
    /// <response code="200">Event updated successfully.</response>
    /// <response code="400">Past events cannot be modified.</response>
    /// <response code="403">User is not authorized to update the event.</response>
    /// <response code="404">Event not found.</response>
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, UpdateEventRequest request)
    {
        var userId =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        await _eventService.UpdateEventAsync(
            id,
            request,
            userId!);

        return Ok("Event updated successfully.");
    }
    /// <summary>
    /// Retrieves events the current user has been invited to.
    /// </summary>
    /// <remarks>
    /// Returns all events where the authenticated user's email
    /// exists in the invitation list.
    ///
    /// Includes private events.
    /// </remarks>
    /// <response code="200">Invited events returned successfully.</response>
    /// <response code="401">Authentication required.</response>
    [Authorize]
    [HttpGet("invited")]
    public async Task<IActionResult> GetInvitedEvents()
    {
        var email =
            User.FindFirstValue(
                ClaimTypes.Email);

        var events =
            await _eventService
                .GetInvitedEventsAsync(email!);

        return Ok(events);
    }
    /// <summary>
    /// Adds a comment to an event.
    /// </summary>
    /// <remarks>
    /// Only authenticated users can add comments.
    /// Comment text is optional.
    /// The comment is associated with the currently logged-in user.
    /// </remarks>
    /// <param name="id">Event identifier.</param>
    /// <param name="request">Comment information.</param>
    /// <response code="200">Comment added successfully.</response>
    /// <response code="401">Authentication required.</response>
    /// <response code="404">Event not found.</response>
    [Authorize]
    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(int id, AddCommentRequest request)
    {
        var userId =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        await _commentService.AddCommentAsync(
            id,
            userId!,
            request);

        return Ok("Comment added successfully.");
    }
}