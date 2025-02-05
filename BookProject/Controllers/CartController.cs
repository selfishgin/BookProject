using Microsoft.AspNetCore.Mvc;
using BookProject.Models;


namespace BookProject.Controllers
{
    public class CartController : Controller
    {
        private const string SessionCartKey = "Cart";

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionCartKey) ?? new List<CartItem>();
            ViewBag.Total = cart.Sum(item => item.Quantity * item.Price);
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int? bookId, int? courseId, int quantity = 1)
        {
            if (quantity <= 0)
            {
                TempData["Error"] = "Quantity must be greater than zero.";
                return RedirectToAction(nameof(Index));
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionCartKey) ?? new List<CartItem>();

            if (bookId.HasValue)
            {
                var book = GetBookById(bookId.Value);
                if (book != null)
                {
                    AddToCartItem(cart, book.BookId, book.BookName, book.BookPrice, quantity, "Book");
                }
                else
                {
                    TempData["Error"] = "Book not found.";
                    return RedirectToAction("Index", "CartItem");
                }
            }
            else if (courseId.HasValue)
            {
                var course = GetCourseById(courseId.Value);
                if (course != null)
                {
                    AddToCartItem(cart, course.CourseId, course.CourseName, course.CoursePrice, quantity, "Course");
                }
                else
                {
                    TempData["Error"] = "Course not found.";
                    return RedirectToAction("Index", "Courses");
                }
            }
            else
            {
                TempData["Error"] = "Invalid selection.";
                return RedirectToAction(nameof(Index));
            }

            HttpContext.Session.SetObjectAsJson(SessionCartKey, cart);
            TempData["Success"] = "Item added to cart successfully.";

            return RedirectToAction(nameof(Index)); 
        }

        
        [HttpPost]
        public IActionResult RemoveFromCart(int itemId, string itemType)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionCartKey) ?? new List<CartItem>();
            var itemToRemove = cart.FirstOrDefault(c => c.ItemId == itemId && c.ItemType == itemType);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                HttpContext.Session.SetObjectAsJson(SessionCartKey, cart);
                TempData["Success"] = "Item removed from cart.";
            }
            else
            {
                TempData["Error"] = "Item not found in the cart.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(SessionCartKey);
            TempData["Success"] = "Cart cleared.";
            return RedirectToAction(nameof(Index));
        }

        private void AddToCartItem(List<CartItem> cart, int itemId, string name, decimal price, int quantity, string type)
        {
            var existingItem = cart.FirstOrDefault(c => c.ItemId == itemId && c.ItemType == type);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ItemId = itemId,
                    ItemName = name,
                    Price = price,
                    Quantity = quantity,
                    ItemType = type
                });
            }
        }

        private Book GetBookById(int bookId) => new Book { BookId = bookId, BookName = "Sample Book", BookPrice = 19.99M };
        private Course GetCourseById(int courseId) => new Course { CourseId = courseId, CourseName = "Sample Course", CoursePrice = 49.99M };
    }
}
