using BookReadingEvents.Application.DTOs.Requests;
using BookReadingEvents.Application.Exceptions;
using BookReadingEvents.Application.Interfaces;
using BookReadingEvents.Application.Services;
using BookReadingEvents.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace BookReadingEvents.Tests.Services;

public class AuthServiceTests
{
    private Mock<UserManager<ApplicationUser>> MockUserManager()
    {
        var store =
            new Mock<IUserStore<ApplicationUser>>();

        return new Mock<UserManager<ApplicationUser>>(
            store.Object,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!);
    }

    [Fact]
    public async Task Register_Should_Throw_When_Email_Already_Exists()
    {
        // Arrange
        var userManagerMock = MockUserManager();

        userManagerMock
            .Setup(x =>
                x.FindByEmailAsync("test@gmail.com"))
            .ReturnsAsync(
                new ApplicationUser
                {
                    Email = "test@gmail.com"
                });

        var jwtServiceMock =
            new Mock<IJwtService>();

        var authService =
            new AuthService(
                userManagerMock.Object,
                jwtServiceMock.Object);

        var request =
            new RegisterRequest
            {
                Email = "test@gmail.com",
                Password = "Test@1",
                FullName = "Test User"
            };

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(
            () => authService.RegisterAsync(request));
    }

    [Fact]
    public async Task Login_Should_Throw_When_User_Not_Found()
    {
        // Arrange
        var userManagerMock = MockUserManager();

        userManagerMock
            .Setup(x =>
                x.FindByEmailAsync("test@gmail.com"))
            .ReturnsAsync((ApplicationUser?)null);

        var jwtServiceMock =
            new Mock<IJwtService>();

        var authService =
            new AuthService(
                userManagerMock.Object,
                jwtServiceMock.Object);

        var request =
            new LoginRequest
            {
                Email = "test@gmail.com",
                Password = "Test@1"
            };

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(
            () => authService.LoginAsync(request));
    }
}