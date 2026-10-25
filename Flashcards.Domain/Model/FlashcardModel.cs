using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.Domain.Model
{
    /// <summary>
    /// Represents a flashcard entity within the domain.
    /// </summary>
    public class FlashcardModel : BaseModel 
    {
        public Guid BoxId { get; set; }

        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        public int Level { get; set; }
        
        public DateOnly DueDate { get; set; }

        public DateOnly? LastAnsweredOn { get; set; }
        
        public bool? LastAnswerWasCorrect { get; set; }
    }
}
