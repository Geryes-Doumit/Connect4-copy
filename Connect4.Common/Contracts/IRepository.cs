namespace Connect4.Common.Contracts;

/// <summary>
/// Defines the write operations for a repository of entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The entity type handled by this repository.</typeparam>
public interface IRepository<T> where T : Entity
{
    /// <summary>
    /// Creates a new entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to be created.</param>
    /// <returns>An asynchronous task that resolves to the identifier of the newly created entity.</returns>
    Task<int> Create(T entity);

    /// <summary>
    /// Updates an existing entity in the repository with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to update.</param>
    /// <param name="entity">The entity data to apply.</param>
    Task Update(int id, T entity);

    /// <summary>
    /// Permanently deletes an entity from the repository.
    /// </summary>
    /// <param name="id">The identifier of the entity to delete.</param>
    Task Delete(int id);
}
