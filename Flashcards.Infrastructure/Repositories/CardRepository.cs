using Flashcards.Application.Abstractions;
using Flashcards.Domain.Model;
using Flashcards.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Infrastructure.Repositories;

/// <summary>
///     Provides Entity Framework Core based persistence for <see cref="CardModel" /> entities.
/// </summary>
/// <param name="dbContext">The request-scoped database context.</param>
internal class CardRepository(FlashcardsDbContext dbContext) : ICardRepository
{
    /// <summary>
    /// Adds a new card to the database and saves the changes.
    /// </summary>
    /// <param name="value">The card to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public async Task AddAsync(CardModel value, CancellationToken cancellationToken = default)
    {
        dbContext.Cards.Add(value);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Removes a card from the database and saves the changes.
    /// </summary>
    /// <param name="value">The card to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public async Task DeleteAsync(CardModel value, CancellationToken cancellationToken = default)
    {
        dbContext.Cards.Remove(value);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Saves the changes made to a card. A card that is not tracked by the context is attached and marked as modified.
    /// </summary>
    /// <param name="value">The card to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    public async Task UpdateAsync(CardModel value, CancellationToken cancellationToken = default)
    {
        // Im selben Request per GetAsync geladen: Change Tracking erkennt die geänderten Felder.
        // Update() nur für nicht getrackte Entitäten (z. B. von außen übergeben).
        if (dbContext.Entry(value).State == EntityState.Detached)
            dbContext.Cards.Update(value);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Gets a card by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the card.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The card, or <c>null</c> if no card with this identifier exists.</returns>
    public async Task<CardModel?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Cards.FindAsync([id], cancellationToken);
    }
}
