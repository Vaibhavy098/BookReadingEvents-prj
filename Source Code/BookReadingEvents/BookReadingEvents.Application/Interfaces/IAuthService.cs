using BookReadingEvents.Application.DTOs.Requests;
using BookReadingEvents.Application.DTOs.Responses;

namespace BookReadingEvents.Application.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);

    Task<AuthResponse> LoginAsync(LoginRequest request);
}