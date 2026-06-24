using BookReadingEvents.Application.DTOs.Requests;
using BookReadingEvents.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace BookReadingEvents.API.Controllers;

[ApiController] // Indicates that this class is an API controller, enabling features like automatic model validation and response formatting.
[Route("api/[controller]")]
public class AuthController : ControllerBase // controllerbase is a base class for API controllers that provides common functionality for handling HTTP requests and responses, without the view-rendering capabilities of the Controller class. Like -> ok(), badRequest(), Unauthorized(), NotFound()
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <remarks>
    /// Creates a new user using ASP.NET Identity.
    /// Email must be unique.
    /// Password must contain at least one special character.
    /// </remarks>
    /// <param name="request">Registration details.</param>
    /// <response code="200">User registered successfully.</response>
    /// <response code="400">Validation failed.</response>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        await _authService.RegisterAsync(request);

        return Ok("User registered successfully.");
    }
    
    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <remarks>
    /// Use the returned JWT token in the Authorization header
    /// for accessing protected endpoints.
    /// </remarks>
    /// <param name="request">Login credentials.</param>
    /// <response code="200">Login successful.</response>
    /// <response code="403">Invalid credentials.</response>
    /// 
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest request)
    {
        var response =
            await _authService.LoginAsync(request);

        return Ok(response);
    }
    
}