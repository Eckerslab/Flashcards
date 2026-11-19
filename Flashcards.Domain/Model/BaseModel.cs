namespace Flashcards.Domain.Model;

/// <summary>
///     Represents the base model for domain entities, providing common properties and functionality.
/// </summary>
public class BaseModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public Guid Id { get; set; }
}