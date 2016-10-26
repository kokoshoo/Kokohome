using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Decks;
using MySite.DataContexts;
using Microsoft.AspNet.Identity;
using MySite.Models;
using System.Data.Entity.Migrations;
using MySite.Attributes;

namespace MySite.Controllers
{
    public class DecksController : Controller
    {
        private DecksDb db = new DecksDb();


        // GET: Decks
        public async Task<ActionResult> Index()
        {
            return View(await db.Decks.ToListAsync());
        }

        [Authorize]
        public ActionResult MyDecks()
        {
            var decks = db.GetDecksForUser(User.Identity.GetUserId());
            return View("MyDecks", decks);
        }

        // GET: Decks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deck deck = await db.Decks.FindAsync(id);
            if (deck == null)
            {
                return HttpNotFound();
            }

            return View(deck);
        }

        // GET: Decks/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Decks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,DateCreated,DateUpdated,UserId")] Deck deck)
        {
            if (ModelState.IsValid)
            {
                deck.UserId = User.Identity.GetUserId();
                deck.DateCreated = DateTime.Now;
                deck.DateUpdated = DateTime.Now;
                db.Decks.Add(deck);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View("MyDecks",deck);
        }

        [Authorize]
        public async Task<ActionResult> CreateFlashcard([Bind(Include = "CardFront,CardBack, DeckId")] Flashcard flashcard)
        {
            if (ModelState.IsValid)
            {
                if (!db.UserOwnsDeck(User.Identity.GetUserId(), flashcard.DeckId)) //Somebody is creating flashcards in a deck they don't own, send them home
                    return RedirectToAction("Index");

                db.Flashcards.Add(flashcard);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Edit", new { id = flashcard.DeckId });

        }

        public async Task<ActionResult> Study(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deck deck = await db.Decks.FindAsync(id);
            deck.Flashcards = db.GetFlashcardsForDeck(id);
            if (deck == null)
            {
                return HttpNotFound();
            }

            return View(deck);
        }

        // GET: Decks/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deck deck = await db.Decks.FindAsync(id);
            List<Flashcard> flashcards = db.GetFlashcardsForDeck(id);
            if (deck == null)
            {
                return HttpNotFound();
            }
            ViewBag.Flashcards = flashcards;
            deck.Flashcards = flashcards;
            return View(deck);
        }

        // POST: Decks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,DateCreated,DateUpdated")] Deck deck)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                if (!db.UserOwnsDeck(userId, deck.Id)) //Somebody is editing a deck they don't own, send them home
                    return RedirectToAction("Index");
                deck.DateUpdated = DateTime.Now;
                deck.UserId = userId;
                db.Set<Deck>().AddOrUpdate(deck);
                await db.SaveChangesAsync();
                //return RedirectToAction("Edit", new { id = deck.Id });
            }
            deck.Flashcards = db.GetFlashcardsForDeck(deck.Id);
            return View(deck);
        }

        [Authorize]
        public async Task<ActionResult> EditFlashcard(List<Flashcard> flashcards)
        {
            if (ModelState.IsValid)
            {
                string UserId = User.Identity.GetUserId();
                if (!db.FlashcardBelongsInDeck(flashcards) || !db.UserOwnsDeck(UserId, flashcards)) //Somebody is trying to change a card that from a different deck, send them home
                    return RedirectToAction("Index");

                foreach (var flashcard in flashcards)
                {
                    //db.Entry(flashcard).State = EntityState.Modified;
                    db.Set<Flashcard>().AddOrUpdate(flashcard);
                }
                await db.SaveChangesAsync();
                TempData["FlashcardUpdateSuccess"] = "Success!";
                return RedirectToAction("Edit", new { id = flashcards.First().DeckId });
            }
            TempData["FlashcardUpdateFail"] = "Failed!";
            return View(flashcards);

        }

        // GET: Decks/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Deck deck = await db.Decks.FindAsync(id);
            if (deck == null)
            {
                return HttpNotFound();
            }

            if (!db.UserOwnsDeck(User.Identity.GetUserId(), deck.Id)) //Somebody is deleting a deck they don't own, send them home
                return RedirectToAction("Index");

            return View(deck);
        }

        // POST: Decks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Deck deck = await db.Decks.FindAsync(id);

            if (!db.UserOwnsDeck(User.Identity.GetUserId(), deck.Id)) //Somebody is deleting a deck they don't own, send them home
                return RedirectToAction("Index");

            db.Decks.Remove(deck);
            //Get all flashcards under the deckid and remove them as well
            var cards = db.GetFlashcardsForDeck(id);
            db.Flashcards.RemoveRange(cards);

            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<ActionResult> DeleteFlashcard(int id)
        {
            if (!db.UserOwnsFlashcard(User.Identity.GetUserId(), id)) //Somebody is trying to change a card that from a different deck, send them home
                return RedirectToAction("Index");

            Flashcard card = await db.Flashcards.FindAsync(id);
            db.Flashcards.Remove(card);
            await db.SaveChangesAsync();

            return RedirectToAction("Edit", new { id = card.DeckId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
