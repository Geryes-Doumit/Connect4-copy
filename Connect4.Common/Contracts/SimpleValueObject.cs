namespace Connect4.Common.Contracts;

/// <summary>
/// Represents a simple value object pattern where <typeparamref name="T"/> is the underlying value type.
/// </summary>
/// <typeparam name="T">The type of the underlying value.</typeparam>
public abstract class SimpleValueObject<T> where T : class
{
    /// <summary>
    /// Gets the validated value for this value object.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleValueObject{T}"/> class.
    /// The passed <paramref name="value"/> is validated by <see cref="Validate"/>.
    /// </summary>
    /// <param name="value">The initial raw value.</param>
    public SimpleValueObject(T value) => Value = Validate(value);

    /// <summary>
    /// Validates the given raw value and returns the possibly corrected/accepted value.
    /// Throws an exception if the value is invalid.
    /// </summary>
    /// <param name="value">The raw value to validate.</param>
    /// <returns>The validated value.</returns>
    public abstract T Validate(T value);
}
