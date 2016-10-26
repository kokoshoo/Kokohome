using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decks
{
    public class Deck
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public List<Flashcard> Flashcards { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public string UserId { get; set; }
    }

    public class Flashcard
    {
        public int Id { get; set; }
        [Required]
        public string CardFront { get; set; }
        public string CardBack { get; set; }
        public int DeckId { get; set; }

        public virtual Deck Deck { get; set; }

    }

}
