using System.Reflection.Metadata.Ecma335;
using Microsoft.VisualBasic;

namespace Flashcards.Domain.Model;

/// <summary>
///     Represents a flashcard entity within the domain.
/// </summary>
public class CardModel : BaseModel
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CardModel" /> class with level 0.
    /// </summary>
    /// <param name="boxId">The identifier of the box the card belongs to.</param>
    /// <param name="question">The question shown on the front of the card.</param>
    /// <param name="answer">The answer shown on the back of the card.</param>
    /// <param name="dueDate">The date on which the card is due for its first review.</param>
    public CardModel(Guid boxId, string question, string answer, DateOnly dueDate)
    {
        BoxId = boxId;
        Question = question;
        Answer = answer;
        Level = 0;
        DueDate = dueDate;
    }

    /// <summary>
    ///     Gets or sets the identifier of the box the card belongs to.
    /// </summary>
    public Guid BoxId { get; set; }

    /// <summary>
    ///     Gets the question shown on the front of the card.
    /// </summary>
    public string Question { get; private set; }

    /// <summary>
    ///     Gets the answer shown on the back of the card.
    /// </summary>
    public string Answer { get; private set; }

    /// <summary>
    ///     Gets or sets the learning level of the card, which determines how often it is repeated.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    ///     Gets or sets the date on which the card is due for its next review.
    /// </summary>
    public DateOnly DueDate { get; set; }

    /// <summary>
    ///     Gets the date on which the card was last answered, or <c>null</c> if it has never been answered.
    /// </summary>
    public DateOnly? LastAnsweredOn { get; private set; }

    /// <summary>
    ///     Gets whether the last answer was correct, or <c>null</c> if the card has never been answered.
    /// </summary>
    public bool? LastAnswerWasCorrect { get; private set; }

    /// <summary>
    ///     Records an answer to the card and updates its learning level.
    ///     A correct answer increases the level by one, a wrong answer resets it to 0.
    /// </summary>
    /// <param name="isCorrect">Indicates whether the answer was correct.</param>
    /// <param name="today">The date on which the answer was given.</param>
    public void RecordAnswer(bool isCorrect, DateOnly today)
    {
        LastAnsweredOn = today;
        LastAnswerWasCorrect = isCorrect;
        if (!IsDueDateToday(today)) return;
        if (isCorrect && Level < 6)
        {
            Level = Level + 1;
            DueDate = today.AddDays(LevelIntervals[Level]);
        }
        else
        {
            Level = 0;
        }
    }
    private  bool IsDueDateToday(DateOnly today)
    {
        return DueDate == today;
    }
    /// <summary>
    /// Intervals, in days, that determine the available flashcard review levels.
    /// </summary>
    private static readonly List<int> LevelIntervals = [1, 3, 5, 7, 14, 21];
}