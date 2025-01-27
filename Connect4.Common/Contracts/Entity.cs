namespace Connect4.Common.Contracts;

/// <summary>
/// Represents the base class for all domain entities.
/// </summary>
/// <param name="id">The unique identifier of the entity.</param>
public abstract class Entity(int id)
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    public int Id { get; set;} = id;
}
