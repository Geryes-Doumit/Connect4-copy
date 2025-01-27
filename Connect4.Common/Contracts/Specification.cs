using System.Linq.Expressions;

namespace Connect4.Common.Contracts;

/// <summary>
/// Represents a specification pattern for filtering or matching entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The entity type to which this specification applies.</typeparam>
public abstract class Specification<T> where T : Entity
{
    /// <summary>
    /// Converts the specification into a LINQ expression that can be used to test an entity.
    /// </summary>
    /// <returns>An expression representing the predicate logic of this specification.</returns>
    public abstract Expression<Func<T, bool>> ToExpression();

    /// <summary>
    /// Determines whether the specified <paramref name="entity"/> satisfies this specification.
    /// </summary>
    /// <param name="entity">The entity to test.</param>
    /// <returns><c>true</c> if the entity satisfies the specification; otherwise <c>false</c>.</returns>
    public bool IsSatisfiedBy(T entity)
    {
        var predicate = ToExpression().Compile();
        return predicate(entity);
    }
}
