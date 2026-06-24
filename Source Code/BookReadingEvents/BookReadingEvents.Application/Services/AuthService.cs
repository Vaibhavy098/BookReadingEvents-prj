using BookReadingEvents.Application.DTOs.Requests;
using BookReadingEvents.Application.DTOs.Responses;
using BookReadingEvents.Application.Exceptions;
using BookReadingEvents.Application.Interfaces;
using BookReadingEvents.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BookReadingEvents.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        var existingUser =
            await _userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
        {
            throw new BusinessRuleException(
                "Email already registered.");
        }

        var user = new ApplicationUser // DTO to entity mapping
        {
            Email = request.Email,
            UserName = request.Email,
            FullName = request.FullName
        };

        var result =  // Create the user with the provided password by hashing it and storing it securely in the database
            await _userManager.CreateAsync(   
                user,
                request.Password);

        if (!result.Succeeded)
        {
            throw new BusinessRuleException(
                string.Join(", ",
                result.Errors.Select(e => e.Description)));
        }

        await _userManager.AddToRoleAsync(user, "User");
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user =
            await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            throw new ForbiddenException(
                "Invalid credentials.");
        }

        var passwordValid =
            await _userManager.CheckPasswordAsync(  // checks if the password provided in the request matches the hashed password stored in the database for the user
                user,
                request.Password);

        if (!passwordValid)
        {
            throw new ForbiddenException(
                "Invalid credentials.");
        }

        var token =
            await _jwtService.GenerateTokenAsync(user);

        return new AuthResponse
        {
            Token = token,
            Expiry = DateTime.UtcNow.AddHours(1)
        };
    }
}