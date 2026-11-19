using Flashcards.Application.Abstractions;
using Flashcards.Domain.Model;
using Flashcards.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Infrastructure.Repositories;

/// <summary>
///     Provides Entity Framework Core based persistence for <see cref="BoxModel" /> entities.
/// </summary>
/// <param name="dbContext">The request-scoped database context.</param>
internal class BoxRepository(FlashcardsDbContext dbContext) : IBoxRepository
{
    /// <summary>
    /// Adds a new box to the database and saves the changes.
    /// </summary>
    /// <param name="value">The box to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public async Task AddAsync(BoxModel value, CancellationToken cancellationToken = default)
    {
        dbContext.Boxes.Add(value);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Removes a box from the database and saves the changes.
    /// </summary>
    /// <param name="value">The box to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public async Task DeleteAsync(BoxModel value, CancellationToken cancellationToken = default)
    {
        dbContext.Boxes.Remove(value);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Saves the changes made to a box. A box that is not tracked by the context is attached and marked as modified.
    /// </summary>
    /// <param name="value">The box to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public async Task UpdateAsync(BoxModel value, CancellationToken cancellationToken = default)
    {
        // Im selben Request per GetAsync geladen: Change Tracking erkennt die geänderten Felder.
        // Update() nur für nicht getrackte Entitäten (z. B. von außen übergeben).
        if (dbContext.Entry(value).State == EntityState.Detached)
            dbContext.Boxes.Update(value);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Gets a box by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the box.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The box, or <c>null</c> if no box with this identifier exists.</returns>
    public async Task<BoxModel?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Boxes.FindAsync([id], cancellationToken);
    }
}
