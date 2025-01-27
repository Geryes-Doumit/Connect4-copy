using System.Collections.Concurrent;

namespace Connect4.Domain.Services;

/// <summary>
/// Defines operations for token revocation via a blacklist mechanism.
/// </summary>
public interface ITokenBlacklistService
{
    /// <summary>
    /// Blacklists a token by its JTI (unique identifier), 
    /// with an expiration time indicating until when it should be considered invalid.
    /// </summary>
    /// <param name="jti">The unique token identifier (JTI claim) to blacklist.</param>
    /// <param name="expiresAt">The expiry date/time for the token.</param>
    void BlacklistToken(string jti, DateTime expiresAt);

    /// <summary>
    /// Checks whether a token with the specified JTI is currently blacklisted.
    /// </summary>
    /// <param name="jti">The token's unique identifier.</param>
    /// <returns><c>true</c> if the token is on the blacklist and not expired; otherwise, <c>false</c>.</returns>
    bool IsTokenBlacklisted(string jti);
}

/// <summary>
/// In-memory implementation of <see cref="ITokenBlacklistService"/>. 
/// Suitable for demo or single-instance scenarios, as the blacklist is not persisted across restarts.
/// </summary>
public class TokenBlacklistService : ITokenBlacklistService
{
    private readonly ConcurrentDictionary<string, DateTime> _blacklistedTokens
        = new ConcurrentDictionary<string, DateTime>();

    /// <inheritdoc/>
    public void BlacklistToken(string jti, DateTime expiresAt)
    {
        _blacklistedTokens[jti] = expiresAt;
    }

    /// <inheritdoc/>
    public bool IsTokenBlacklisted(string jti)
    {
        if (_blacklistedTokens.TryGetValue(jti, out var exp))
        {
            if (DateTime.UtcNow < exp)
            {
                // Token is still within its blacklisted lifetime
                return true;
            }
            else
            {
                // Token has expired, so remove it from the dictionary
                _blacklistedTokens.TryRemove(jti, out _);
                return false;
            }
        }
        return false;
    }
}
