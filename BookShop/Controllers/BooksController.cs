using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly List<Author> authorlist = new List<Author>();
        public BooksController(ApplicationDbContext db)
        {
            _db = db;
            authorlist = _db.Authors.OrderBy(a => a.Name).ToList();
            authorlist.Insert(0, new Author { AuthorId = 0, Name = "Select.." });

        }

        public IActionResult Index()
        {
            return View(_db.Books.ToList());
        }

        public IActionResult Create()
        {
            //List<Author> authorlist = new List<Author>();
            //authorlist = _db.Authors.OrderBy(a => a.Name).ToList();
            //authorlist.Insert(0, new Author { AuthorId = 0, Name = "Select.." });
            ViewBag.ListOfAuthors = authorlist;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _db.Add(book);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(book);
            }
        }
        public IActionResult Details(int? id)
        {
            var book = _db.Books.SingleOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        public IActionResult Edit(int? id)
        {
            var book = _db.Books.SingleOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.ListOfAuthors = authorlist;
            return View(book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Update(book);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public IActionResult Delete(int? id)
        {
            var book = _db.Books.SingleOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBook(int id)
        {
                var book = _db.Books.SingleOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
                _db.Remove(book);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
        }
    }
}