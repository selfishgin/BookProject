using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookProject.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BookProject.Controllers
{
    public class CoursesController : Controller
    {
        private readonly BookProjectDbContext _context;
        private const string SessionCartKey = "Cart"; // Session key for cart items

        public CoursesController(BookProjectDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                TempData["Error"] = "Course not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Course created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to create the course. Please check the form and try again.";
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                TempData["Error"] = "Course not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            if (id != course.CourseId)
            {
                TempData["Error"] = "Invalid course ID.";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Course updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Courses.Any(e => e.CourseId == id))
                    {
                        TempData["Error"] = "Course not found.";
                        return NotFound();
                    }
                    else
                    {
                        TempData["Error"] = "An error occurred while updating the course.";
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Failed to update the course. Please check the form and try again.";
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                TempData["Error"] = "Course not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Course deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Course not found.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Courses/AddToCart
        [HttpPost]
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            if (quantity <= 0)
            {
                TempData["Error"] = "Quantity must be greater than zero.";
                return RedirectToAction(nameof(Index));
            }

            var course = _context.Courses.FirstOrDefault(c => c.CourseId == id);
            if (course == null)
            {
                TempData["Error"] = "Course not found.";
                return RedirectToAction(nameof(Index));
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionCartKey) ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(c => c.ItemId == id && c.ItemType == "Course");
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ItemId = course.CourseId,
                    ItemName = course.CourseName,
                    Price = course.CoursePrice,
                    Quantity = quantity,
                    ItemType = "Course"
                });
            }

            HttpContext.Session.SetObjectAsJson(SessionCartKey, cart);

            TempData["Success"] = $"Course '{course.CourseName}' added to cart.";
            return RedirectToAction(nameof(Index));
        }
    }
}
