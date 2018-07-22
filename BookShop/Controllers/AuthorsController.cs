using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AuthorsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Authors.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create(Author author)
        {
            if (ModelState.IsValid)
            {
                _db.Add(author);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(author);
            }
        }
        public IActionResult Details(int? id)
        {
            var author = _db.Authors.SingleOrDefault(b => b.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        public IActionResult Edit(int? id)
        {
            var author = _db.Authors.SingleOrDefault(b => b.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Author author)
        {
            if (id != author.AuthorId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Update(author);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public IActionResult Delete(int? id)
        {
            var author = _db.Authors.SingleOrDefault(b => b.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAuthor(int id)
        {
                var author = _db.Authors.SingleOrDefault(b => b.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }
                _db.Remove(author);
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