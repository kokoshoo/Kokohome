using Decks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MySite.DataContexts
{
    public class DecksDb : DbContext
    {
        public DecksDb() : base("DefaultConnection")
        {

        }

        public DbSet<Deck> Decks { get; set; }
        public DbSet<Flashcard> Flashcards { get; set; }



        public bool UserOwnsDeck(string UserId, List<Flashcard> cards)
        {
            if (cards.Exists(c => !GetDecksForUser(UserId).Select(d => d.Id).Contains(c.DeckId)))
                return false;
            return true;
        }

        public bool UserOwnsDeck(string UserId, int DeckId)
        {
            if (!GetDecksForUser(UserId).Select(d => d.Id).Contains(DeckId))
                return false;
            return true;
        }

        public bool UserOwnsFlashcard(string UserId, int FlashcardId)
        {
            //User owns Deck && Deck has Flashcard
            if (Decks.Where(d => Flashcards.FirstOrDefault(f => f.Id == FlashcardId).DeckId == d.Id && UserId == d.UserId).Count() > 0)
                return true;
            return false;
        }


        //Only checks against one deck, if multiple passed in will always return false
        internal bool FlashcardBelongsInDeck(List<Flashcard> flashcards)
        {
            if (flashcards.Exists(fl => !GetFlashcardsForDeck(flashcards.First().DeckId).Select(c => c.Id).Contains(fl.Id)))
                return false;
            return true;
        }

        public IEnumerable<Deck> GetDecksForUser(string UserId)
        {
            return Decks.Where(d => d.UserId == UserId);
        }

        public List<Flashcard> GetFlashcardsForDeck(int? DeckId)
        {
            if (DeckId == null)
                throw new Exception("Deck has null id");
            return Flashcards.Where(f => f.DeckId == DeckId).ToList();
        }

        public IQueryable GetCountsByDeckId()
        {
            List<KeyValuePair<int, int>> p = new List<KeyValuePair<int, int>>();
            return Flashcards.GroupBy(f => new { f.DeckId })
                .Select(g => new {g.Key.DeckId, MyCount = g.Count() });
        }

        public bool FlashcardBelongsInDeck(int CardId, int DeckId)
        {
            if (Flashcards.FirstOrDefault(f => f.Id == CardId && f.DeckId == DeckId) != null)
                return true;
            return false;
        }

    }
}