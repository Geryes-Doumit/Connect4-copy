using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace Connect4.Domain.Tests.Mocks;

/// <summary>
/// Mock implementation of <see cref="IConfiguration"/> for testing purposes.
/// </summary>
/// <remarks>
/// This class provides a simple in-memory key-value store to simulate configuration settings during unit tests.
/// </remarks>
public class ConfigurationMock : IConfiguration
{
    private readonly Dictionary<string, string> _settings = new();

    /// <summary>
    /// Gets or sets a configuration value based on the specified key.
    /// </summary>
    /// <param name="key">The key of the configuration value.</param>
    /// <returns>The configuration value associated with the key.</returns>
    public string this[string key]
    {
        get => _settings.ContainsKey(key) ? _settings[key] : throw new KeyNotFoundException($"Key '{key}' not found in the configuration.");
        set => _settings[key] = value;
    }

    /// <summary>
    /// Retrieves the immediate child sections of this configuration.
    /// </summary>
    /// <remarks>
    /// Not implemented in this mock as it is unnecessary for basic key-value testing.
    /// </remarks>
    /// <returns>An enumerable of <see cref="IConfigurationSection"/> representing the child sections.</returns>
    /// <exception cref="NotImplementedException">Always thrown as this is not implemented.</exception>
    public IEnumerable<IConfigurationSection> GetChildren() => throw new NotImplementedException();

    /// <summary>
    /// Gets a change token that can be used to observe when this configuration is reloaded.
    /// </summary>
    /// <remarks>
    /// Not implemented in this mock as dynamic configuration reloading is not required for testing purposes.
    /// </remarks>
    /// <returns>An <see cref="IChangeToken"/> for observing configuration changes.</returns>
    /// <exception cref="NotImplementedException">Always thrown as this is not implemented.</exception>
    public IChangeToken GetReloadToken() => throw new NotImplementedException();

    /// <summary>
    /// Retrieves a configuration section with the specified key.
    /// </summary>
    /// <param name="key">The key of the configuration section to retrieve.</param>
    /// <returns>An <see cref="IConfigurationSection"/> representing the specified section.</returns>
    /// <exception cref="NotImplementedException">Always thrown as this is not implemented.</exception>
    public IConfigurationSection GetSection(string key) => throw new NotImplementedException();
}
