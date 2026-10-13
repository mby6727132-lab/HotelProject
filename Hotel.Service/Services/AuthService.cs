using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Hotel.Core.DTOs.Auth;
using Hotel.Core.Entities;
using Hotel.Core.Enums;
using Hotel.Core.Exceptions;
using Hotel.Core.Interfaces.Repositories;
using Hotel.Core.Interfaces.Services;
using Hotel.Core.Settings;
using Hotel.Service.Security;
using Microsoft.IdentityModel.Tokens;

namespace Hotel.Service.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IUserRepository userRepository, IMapper mapper, JwtSettings jwtSettings)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _jwtSettings = jwtSettings;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _userRepository.GetByEmailAsync(dto.Email, cancellationToken);
        if (existing is not null)
            throw new EmailAlreadyExistsException($"Email '{dto.Email}' is already registered.");

        var user = _mapper.Map<User>(dto);
        user.PasswordHash = PasswordHasher.Hash(dto.Password);
        user.Role = UserRole.Customer.ToString();

        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<AuthResponseDto>(user);
        response.Token = GenerateToken(user);
        return response;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email, cancellationToken);
        if (user is null || !PasswordHasher.Verify(dto.Password, user.PasswordHash))
            throw new InvalidCredentialsException("Invalid email or password.");

        var response = _mapper.Map<AuthResponseDto>(user);
        response.Token = GenerateToken(user);
        return response;
    }

    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
