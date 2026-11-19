namespace Flashcards.Domain.Model;

/// <summary>
///     Represents a flashcard entity within the domain.
/// </summary>
public class CardModel : BaseModel
{
    /// <summary>
    /// Gets or sets the identifier of the box the card belongs to.
    /// </summary>
    public Guid BoxId { get; set; }

    /// <summary>
    /// Gets or sets the question shown on the front of the card.
    /// </summary>
    public string Question { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the answer shown on the back of the card.
    /// </summary>
    public string Answer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the learning level of the card, which determines how often it is repeated.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Gets or sets the date on which the card is due for its next review.
    /// </summary>
    public DateOnly DueDate { get; set; }

    /// <summary>
    /// Gets or sets the date on which the card was last answered, or <c>null</c> if it has never been answered.
    /// </summary>
    public DateOnly? LastAnsweredOn { get; set; }

    /// <summary>
    /// Gets or sets whether the last answer was correct, or <c>null</c> if the card has never been answered.
    /// </summary>
    public bool? LastAnswerWasCorrect { get; set; }
}
