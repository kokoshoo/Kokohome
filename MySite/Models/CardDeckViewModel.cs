using Decks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySite.Models
{
    public class CardDeckViewModel
    {

        public Deck deck { get; set; }

        public CardDeckViewModel(int DeckId)
        {
        }

    }
}