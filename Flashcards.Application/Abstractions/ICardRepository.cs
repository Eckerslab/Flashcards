using Flashcards.Domain.Model;

namespace Flashcards.Application.Abstractions;

/// <summary>
///     Defines the persistence operations for <see cref="CardModel" /> entities.
/// </summary>
public interface ICardRepository : IBaseRepository<CardModel>
{

}
