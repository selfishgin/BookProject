using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookProject.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookProject.Controllers
{
    public class PaymentController : Controller
    {
        private readonly BookProjectDbContext _context;

        public PaymentController(BookProjectDbContext context)
        {
            _context = context;
        }

        // GET: Payment
        public async Task<IActionResult> Index()
        {
            // TODO: Replace with actual logged-in user ID retrieval
            int userId = 1;

            // Fetch cart items for the logged-in user
            var cartItems = await _context.Carts
                .Include(c => c.Book)
                .Include(c => c.Course)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            // Calculate the total amount
            ViewBag.Total = cartItems.Sum(c =>
                (c.Book?.BookPrice ?? 0) * c.Quantity +
                (c.Course?.CoursePrice ?? 0) * c.Quantity);

            return View(cartItems);
        }

        // POST: Complete Payment
        [HttpPost]
        public async Task<IActionResult> CompletePayment()
        {
            // TODO: Replace with actual logged-in user ID retrieval
            int userId = 1;

            // Fetch cart items for the logged-in user
            var cartItems = await _context.Carts
                .Include(c => c.Book)
                .Include(c => c.Course)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            // Check if the cart is empty
            if (!cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty. Please add items before checking out.";
                return RedirectToAction("Index", "Cart");
            }

            // Create a new order
            var order = new Order
            {
                UserId = userId,
                TotalAmount = cartItems.Sum(c =>
                    (c.Book?.BookPrice ?? 0) * c.Quantity +
                    (c.Course?.CoursePrice ?? 0) * c.Quantity),
                OrderDate = DateTime.Now
            };

            // Save the order
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Save order items
            foreach (var item in cartItems)
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.OrderId,
                    BookId = item.BookId,
                    CourseId = item.CourseId,
                    Quantity = item.Quantity,
                    Price = item.Book?.BookPrice ?? item.Course?.CoursePrice ?? 0
                });
            }

            // Remove items from the cart
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Payment completed successfully!";
            return RedirectToAction("Index", "Home");
        }
    }
}
