using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace TadaTodo.Server.Models;

[PublicAPI]
public class User
{
    private readonly PasswordHasher<User> _hasher = new();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    // EF Core Constructor to restore persisted data.
    private User()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public User(string username, string password)
    {
        Username = username;
        Password = _hasher.HashPassword(this, password);
    }

    public int Id { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }

    public List<TodoList> TodoLists { get; private set; } = [];

    public void UpdatePassword(string password)
    {
        Password = _hasher.HashPassword(this, password);
    }

    public bool VerifyPassword(string providedPassword)
    {
        return _hasher.VerifyHashedPassword(this, Password, providedPassword) switch
        {
            PasswordVerificationResult.Failed => false,
            PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded => true,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}