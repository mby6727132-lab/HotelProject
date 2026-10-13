using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Hotel.Core.DTOs.Auth;
using Hotel.Core.Entities;
using Hotel.Core.Exceptions;
using Hotel.Core.Interfaces.Repositories;
using Hotel.Core.Settings;
using Hotel.Service.Mapping;
using Hotel.Service.Services;
using Moq;

namespace Hotel.Tests.Services;

public class AuthServiceTests
{
    // Real PBKDF2 hash (same algorithm as PasswordHasher) of "Passw0rd!" - same value used in the seed data.
    private const string DemoPasswordHash = "100000.mF3p0hT9c2vQd8bRzL1yAw==.TiLc1SeDoXndtQJguBGGhZ4m/lGYX+vEELXU9TttdjE=";

    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        var jwtSettings = new JwtSettings
        {
            Key = "unit-test-signing-key-needs-to-be-at-least-32-bytes!",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiryMinutes = 60
        };
        _sut = new AuthService(_userRepository.Object, mapper, jwtSettings);
    }

    [Fact]
    public async Task RegisterAsync_HashesPassword_AndDefaultsRoleToCustomer()
    {
        _userRepository.Setup(r => r.GetByEmailAsync("new@user.com", It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);
        User? captured = null;
        _userRepository.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((u, _) => captured = u)
            .Returns(Task.CompletedTask);
        _userRepository.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var dto = new RegisterDto { Name = "New User", Email = "new@user.com", Password = "Passw0rd!" };
        var result = await _sut.RegisterAsync(dto, CancellationToken.None);

        Assert.NotNull(captured);
        Assert.Equal("Customer", captured!.Role);
        Assert.DoesNotContain(dto.Password, captured.PasswordHash);
        Assert.NotEqual(dto.Password, captured.PasswordHash);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task RegisterAsync_ThrowsEmailAlreadyExistsException_WhenEmailTaken()
    {
        _userRepository.Setup(r => r.GetByEmailAsync("taken@user.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { Id = 1, Name = "x", Email = "taken@user.com", PasswordHash = "h", Role = "Customer" });

        var dto = new RegisterDto { Name = "New User", Email = "taken@user.com", Password = "Passw0rd!" };

        await Assert.ThrowsAsync<EmailAlreadyExistsException>(() => _sut.RegisterAsync(dto, CancellationToken.None));
        _userRepository.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_ReturnsTokenWithUserIdEmailAndRoleClaims_WhenCredentialsValid()
    {
        _userRepository.Setup(r => r.GetByEmailAsync("admin@hotelproject.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { Id = 7, Name = "Admin User", Email = "admin@hotelproject.com", PasswordHash = DemoPasswordHash, Role = "Admin" });

        var result = await _sut.LoginAsync(new LoginDto { Email = "admin@hotelproject.com", Password = "Passw0rd!" }, CancellationToken.None);

        Assert.False(string.IsNullOrWhiteSpace(result.Token));
        Assert.Equal("Admin User", result.Name);
        Assert.Equal("Admin", result.Role);

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(result.Token);
        Assert.Equal("7", jwt.Claims.First(c => c.Type is "nameid" or ClaimTypes.NameIdentifier).Value);
        Assert.Equal("admin@hotelproject.com", jwt.Claims.First(c => c.Type is "email" or ClaimTypes.Email).Value);
        Assert.Equal("Admin", jwt.Claims.First(c => c.Type is "role" or ClaimTypes.Role).Value);
    }

    [Fact]
    public async Task LoginAsync_ThrowsInvalidCredentialsException_WhenPasswordWrong()
    {
        _userRepository.Setup(r => r.GetByEmailAsync("admin@hotelproject.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { Id = 7, Name = "Admin User", Email = "admin@hotelproject.com", PasswordHash = DemoPasswordHash, Role = "Admin" });

        await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
            _sut.LoginAsync(new LoginDto { Email = "admin@hotelproject.com", Password = "WrongPassword" }, CancellationToken.None));
    }

    [Fact]
    public async Task LoginAsync_ThrowsInvalidCredentialsException_WhenEmailNotFound()
    {
        _userRepository.Setup(r => r.GetByEmailAsync("nobody@user.com", It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
            _sut.LoginAsync(new LoginDto { Email = "nobody@user.com", Password = "whatever" }, CancellationToken.None));
    }
}
