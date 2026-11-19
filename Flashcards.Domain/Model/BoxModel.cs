namespace Flashcards.Domain.Model;

/// <summary>
///     Represents a box that groups flashcards belonging to a user.
/// </summary>
public class BoxModel : BaseModel
{
    /// <summary>
    /// Gets or sets the display name of the box.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the user who owns the box.
    /// </summary>
    public string UserId { get; set; } = string.Empty;
}
