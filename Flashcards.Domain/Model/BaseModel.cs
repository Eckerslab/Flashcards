using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.Domain.Model
{
    /// <summary>
    /// Represents the base model for domain entities, providing common properties and functionality.
    /// </summary>
    public class BaseModel
    {
           public Guid Id { get; set; }
    }
}
