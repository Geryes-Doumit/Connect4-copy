namespace Connect4.Common.Contracts;

/// <summary>
/// Serves as a base Data Transfer Object class that wraps an entity of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The entity type associated with the DTO.</typeparam>
/// <param name="entity">The underlying entity instance to be wrapped.</param>
public abstract class Dto<T>(T entity) where T : Entity
{

}
