namespace Flashcards.Application.Abstractions;

/// <summary>
///     Defines the basic persistence operations for a domain entity.
/// </summary>
/// <typeparam name="TEntity">The type of the domain entity.</typeparam>
public interface IBaseRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Adds a new entity and saves the changes.
    /// </summary>
    /// <param name="value">The entity to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task AddAsync(TEntity value, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity and saves the changes.
    /// </summary>
    /// <param name="value">The entity to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task DeleteAsync(TEntity value, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves the changes made to an entity.
    /// </summary>
    /// <param name="value">The entity to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public Task UpdateAsync(TEntity value, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The entity, or <c>null</c> if no entity with this identifier exists.</returns>
    public Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
