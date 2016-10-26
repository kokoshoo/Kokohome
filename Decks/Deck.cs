using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decks
{
    public partial class Deck
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public ICollection<Flashcard> Flashcards { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DataUpdated { get; set; }
        public virtual string UserId { get; set; }

        public virtual User
    }

    public partial class Flashcard
    {
        public int Id { get; set; }
        [Required]
        public string CardFront { get; set; }
        public string CardBack { get; set; }
        public int DeckId { get; set; }

        public virtual Deck Deck { get; set; }

    }
}
