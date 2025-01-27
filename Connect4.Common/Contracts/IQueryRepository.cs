namespace Connect4.Common.Contracts;

/// <summary>
/// Defines the read-only operations for a repository of entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The entity type handled by this repository.</typeparam>
public interface IQueryRepository<T> where T : Entity
{
    Task<IEnumerable<TDto>> FindAll<TDto>(Specification<T> specification)
        where TDto : Dto<T>;
    /// <summary>
    /// Retrieves a collection of DTOs of type <typeparamref name="TDto"/> matching a given specification,
    /// with support for pagination.
    /// </summary>
    /// <typeparam name="TDto">A Data Transfer Object (DTO) that wraps or represents the entity.</typeparam>
    /// <param name="limit">The maximum number of items to retrieve.</param>
    /// <param name="offset">The zero-based offset for items to skip.</param>
    /// <param name="specification">A specification object for filtering the entities.</param>
    /// <returns>An asynchronous task that resolves to a collection of <typeparamref name="TDto"/>.</returns>
    Task<IEnumerable<TDto>> Find<TDto>(int limit, int offset, Specification<T> specification)
        where TDto : Dto<T>;

    /// <summary>
    /// Retrieves a single DTO that represents the entity with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <returns>An asynchronous task that resolves to a <see cref="Dto{T}"/>.</returns>
    Task<Dto<T>> GetOne(int id);
}
