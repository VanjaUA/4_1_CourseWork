using System.Security.Cryptography;
using System.Text;
using Warehouse.Application.Interfaces;

namespace Warehouse.Infrastructure.Auth;

public class PasswordHasher : IPasswordHasher
{
    // Simple SHA256 hasher for demonstration. 
    // In production, use BCrypt or PBKDF2 (e.g. ASP.NET Core Identity PasswordHasher)
    public string Hash(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    public bool Verify(string password, string hash)
    {
        return Hash(password) == hash;
    }
}
