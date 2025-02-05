using System.Diagnostics;
using BookProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookProject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}
