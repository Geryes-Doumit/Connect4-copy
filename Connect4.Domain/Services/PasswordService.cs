using System.Security.Cryptography;

namespace Connect4.Domain.Services;

/// <summary>
/// Defines operations for hashing and verifying passwords.
/// </summary>
public interface IPasswordService
{
    /// <summary>
    /// Hashes a plaintext password using a secure algorithm (PBKDF2).
    /// </summary>
    /// <param name="password">The plaintext password.</param>
    /// <returns>A base64-encoded hash string containing both salt and derived key.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies a plaintext password against a stored hash.
    /// </summary>
    /// <param name="password">The plaintext password to verify.</param>
    /// <param name="storedHash">The stored base64-encoded hash containing salt and derived key.</param>
    /// <returns><c>true</c> if the password matches the stored hash; otherwise, <c>false</c>.</returns>
    bool VerifyPassword(string password, string storedHash);
}

/// <summary>
/// A PBKDF2-based implementation of <see cref="IPasswordService"/>.
/// </summary>
public class PasswordService : IPasswordService
{
    private const int SaltSize = 16;   // 128 bits
    private const int KeySize = 32;    // 256 bits
    private const int Iterations = 600_000; // NIST recommendation

    /// <inheritdoc/>
    public string HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        var key = pbkdf2.GetBytes(KeySize);

        var hashBytes = new byte[SaltSize + KeySize];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
        Buffer.BlockCopy(key, 0, hashBytes, SaltSize, KeySize);

        return Convert.ToBase64String(hashBytes);
    }

    /// <inheritdoc/>
    public bool VerifyPassword(string password, string storedHash)
    {
        var hashBytes = Convert.FromBase64String(storedHash);

        var salt = new byte[SaltSize];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

        var storedKey = new byte[KeySize];
        Buffer.BlockCopy(hashBytes, SaltSize, storedKey, 0, KeySize);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        var computedKey = pbkdf2.GetBytes(KeySize);

        return CryptographicOperations.FixedTimeEquals(storedKey, computedKey);
    }
}
