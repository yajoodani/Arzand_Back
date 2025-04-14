using Arzand.Modules.Identity.DTOs;
using Arzand.Modules.Identity.Models;
using Arzand.Modules.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

public class AuthServiceTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        var userStoreMock = new Mock<IUserStore<AppUser>>();
        _userManagerMock = new Mock<UserManager<AppUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        _configMock = new Mock<IConfiguration>();

        _configMock.Setup(x => x["Jwt:Secret"]).Returns("super-secret-jwt-key-for-testing123!");
        _configMock.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
        _configMock.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");

        _authService = new AuthService(_userManagerMock.Object, _configMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ReturnsAuthResponse_WhenSuccessful()
    {
        // Arrange
        var request = new RegisterRequest { Email = "user@example.com", Password = "password123" };
        var appUser = new AppUser { Email = request.Email };

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), request.Password))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), "Customer"))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<AppUser>()))
            .ReturnsAsync(new List<string> { "Customer" });

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Email, result.Email);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task LoginAsync_ReturnsNull_WhenUserNotFound()
    {
        // Arrange
        var request = new LoginRequest { Email = "nonexistent@example.com", Password = "pass" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((AppUser)null!);

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_ReturnsAuthResponse_WhenSuccessful()
    {
        // Arrange
        var request = new LoginRequest { Email = "user@example.com", Password = "pass" };
        var appUser = new AppUser { Email = request.Email };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(appUser);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(appUser, request.Password)).ReturnsAsync(true);
        _userManagerMock.Setup(x => x.GetRolesAsync(appUser)).ReturnsAsync(new List<string> { "Customer" });

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Email, result.Email);
    }
}
