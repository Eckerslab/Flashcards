using Flashcards.Domain.Model;

namespace Flashcards.Domain.Tests;

/// <summary>
///     Contains unit tests for the <see cref="CardModel" /> class.
/// </summary>
public class CardModelTest
{
    private static readonly DateOnly Today = new(2026, 11, 24);

    /// <summary>
    ///     Creates a new card with default test values that is due today.
    /// </summary>
    /// <returns>A new <see cref="CardModel" /> instance.</returns>
    private static CardModel CreateCard()
    {
        return new CardModel(Guid.NewGuid(), "What is TCP?", "Transmission Control Protocol", Today);
    }

    /// <summary>
    ///     Verifies that the constructor assigns all passed values to the corresponding properties.
    /// </summary>
    [Fact]
    public void Constructor_AssignsPassedValues()
    {
        var boxId = Guid.NewGuid();

        var card = new CardModel(boxId, "Question", "Answer", Today);

        Assert.Equal(boxId, card.BoxId);
        Assert.Equal("Question", card.Question);
        Assert.Equal("Answer", card.Answer);
        Assert.Equal(Today, card.DueDate);
    }

    /// <summary>
    ///     Verifies that a new card starts at level 0 and has never been answered.
    /// </summary>
    [Fact]
    public void Constructor_InitializesLearningState()
    {
        var card = CreateCard();

        Assert.Equal(0, card.Level);
        Assert.Null(card.LastAnsweredOn);
        Assert.Null(card.LastAnswerWasCorrect);
    }

    /// <summary>
    ///     Verifies that every new card receives its own non-empty identifier.
    /// </summary>
    [Fact]
    public void Constructor_GeneratesUniqueId()
    {
        var first = CreateCard();
        var second = CreateCard();

        Assert.NotEqual(Guid.Empty, first.Id);
        Assert.NotEqual(first.Id, second.Id);
    }

    /// <summary>
    ///     Verifies that recording an answer stores the answer date and result.
    /// </summary>
    /// <param name="isCorrect">Indicates whether the answer was correct.</param>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void RecordAnswer_StoresLastAnswer(bool isCorrect)
    {
        var card = CreateCard();

        card.RecordAnswer(isCorrect, Today);

        Assert.Equal(Today, card.LastAnsweredOn);
        Assert.Equal(isCorrect, card.LastAnswerWasCorrect);
    }

    /// <summary>
    ///     Verifies that a correct answer on a due card raises the level by one.
    /// </summary>
    [Fact]
    public void RecordAnswer_CorrectAndDue_IncreasesLevel()
    {
        var card = CreateCard();

        card.RecordAnswer(true, Today);

        Assert.Equal(1, card.Level);
    }

    /// <summary>
    ///     Verifies that a correct answer on a due card moves the due date by the interval
    ///     of the reached level (1, 3, 5, 7, 14, 21 days).
    /// </summary>
    /// <param name="startLevel">The level of the card before the answer.</param>
    /// <param name="expectedDays">The expected number of days until the next review.</param>
    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 3)]
    [InlineData(2, 5)]
    [InlineData(3, 7)]
    [InlineData(4, 14)]
    [InlineData(5, 21)]
    public void RecordAnswer_CorrectAndDue_SetsDueDateByInterval(int startLevel, int expectedDays)
    {
        var card = CreateCard();
        card.Level = startLevel;

        card.RecordAnswer(true, Today);

        Assert.Equal(startLevel + 1, card.Level);
        Assert.Equal(Today.AddDays(expectedDays), card.DueDate);
    }

    /// <summary>
    ///     Verifies that a correct answer on a card at the highest level keeps the level
    ///     and schedules the next review with the longest interval.
    /// </summary>
    [Fact]
    public void RecordAnswer_CorrectAtMaxLevel_KeepsMaxLevel()
    {
        var card = CreateCard();
        card.Level = 6;

        card.RecordAnswer(true, Today);

        Assert.Equal(6, card.Level);
        Assert.Equal(Today.AddDays(21), card.DueDate);
    }

    /// <summary>
    ///     Verifies that a wrong answer on a due card resets the level to 0
    ///     and keeps the card due today, so it is repeated in the same session.
    /// </summary>
    /// <param name="startLevel">The level of the card before the answer.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    [InlineData(6)]
    public void RecordAnswer_WrongAndDue_ResetsLevelAndKeepsDueDate(int startLevel)
    {
        var card = CreateCard();
        card.Level = startLevel;

        card.RecordAnswer(false, Today);

        Assert.Equal(0, card.Level);
        Assert.Equal(Today, card.DueDate);
    }

    /// <summary>
    ///     Verifies that answering a card that is not due changes neither its level nor its due date.
    /// </summary>
    /// <param name="isCorrect">Indicates whether the answer was correct.</param>
    /// <param name="dueInDays">The offset of the due date relative to today.</param>
    [Theory]
    [InlineData(true, 3)]
    [InlineData(false, 3)]
    [InlineData(true, -2)]
    [InlineData(false, -2)]
    public void RecordAnswer_NotDue_KeepsLevelAndDueDate(bool isCorrect, int dueInDays)
    {
        var card = CreateCard();
        card.Level = 2;
        card.DueDate = Today.AddDays(dueInDays);

        card.RecordAnswer(isCorrect, Today);

        Assert.Equal(2, card.Level);
        Assert.Equal(Today.AddDays(dueInDays), card.DueDate);
        Assert.Equal(Today, card.LastAnsweredOn);
        Assert.Equal(isCorrect, card.LastAnswerWasCorrect);
    }

    /// <summary>
    ///     Verifies that a card answered correctly on each due date climbs through all levels.
    /// </summary>
    [Fact]
    public void RecordAnswer_CorrectOnEveryDueDate_ReachesMaxLevel()
    {
        var card = CreateCard();

        for (var i = 0; i < 6; i++)
        {
            card.RecordAnswer(true, card.DueDate);
        }

        Assert.Equal(6, card.Level);
        Assert.Equal(Today.AddDays(1 + 3 + 5 + 7 + 14 + 21), card.DueDate);
    }
}
