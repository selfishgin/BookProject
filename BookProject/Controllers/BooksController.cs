using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookProject.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BookProject.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookProjectDbContext _context;
        private const string SessionCartKey = "Cart"; // Session key for cart items

        public BooksController(BookProjectDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.ToListAsync();
            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                TempData["Error"] = "Book not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Book created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to create the book. Please check the form and try again.";
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                TempData["Error"] = "Book not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.BookId)
            {
                TempData["Error"] = "Invalid book ID.";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Book updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Books.Any(e => e.BookId == id))
                    {
                        TempData["Error"] = "Book not found.";
                        return NotFound();
                    }
                    else
                    {
                        TempData["Error"] = "An error occurred while updating the book.";
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to update the book. Please check the form and try again.";
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                TempData["Error"] = "Book not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Book deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Book not found.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Books/AddToCart
        [HttpPost]
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            if (quantity <= 0)
            {
                TempData["Error"] = "Quantity must be greater than zero.";
                return RedirectToAction(nameof(Index));
            }

            var book = _context.Books.FirstOrDefault(b => b.BookId == id);
            if (book == null)
            {
                TempData["Error"] = "Book not found.";
                return RedirectToAction(nameof(Index));
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionCartKey) ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(c => c.ItemId == id && c.ItemType == "Book");
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ItemId = book.BookId,
                    ItemName = book.BookName,
                    Price = book.BookPrice,
                    Quantity = quantity,
                    ItemType = "Book"
                });
            }

            HttpContext.Session.SetObjectAsJson(SessionCartKey, cart);

            TempData["Success"] = $"Book '{book.BookName}' added to cart.";
            return RedirectToAction(nameof(Index));
        }
    }
}
