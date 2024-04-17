using Microsoft.AspNetCore.Mvc;
using PawsCare.Data;
using PawsCare.Models;
using System.Diagnostics;

namespace PawsCare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        public IActionResult Contacts()
        {
            return View();
        }

        public IActionResult SubmitContactForm(Contact contacts)
        {
            if(!ModelState.IsValid)
            {
                return View("Contacts", contacts);
            }
            _context.Contacts.Add(contacts);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Thank you for contacting us, we will get back to you as soon as possible!";
            return RedirectToAction("Contacts");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}