using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Enums;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Application.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepo;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public AuthService(IRepository<User> userRepo, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _userRepo = userRepo;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var users = await _userRepo.FindAsync(u => u.Email == request.Email);
        var user = users.FirstOrDefault();

        if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new Exception("Invalid email or password");
        }

        var token = _jwtProvider.Generate(user);
        return new LoginResponse(token, user.Role.ToString());
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        var existing = await _userRepo.FindAsync(u => u.Email == request.Email);
        if (existing.Any())
        {
            throw new Exception("User already exists");
        }

        var user = new User
        {
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            Role = Role.User // Default role
        };

        await _userRepo.AddAsync(user);
    }
}
