using Warehouse.Domain.Entities;

namespace Warehouse.Application.Interfaces;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}

public interface IJwtProvider
{
    string Generate(User user);
}
