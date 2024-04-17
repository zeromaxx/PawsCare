using Microsoft.AspNetCore.Mvc;
using PawsCare.Data;

namespace PawsCare.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        public ProductsController(AppDbContext context) 
        {
            _context = context;
        }
        public IActionResult Details(int id)
        {
			var product = _context.Products.Where(p => p.Id == id).FirstOrDefault();
			if (product == null)
            {
			   return RedirectToAction("Index", "Home");
			}
            
            return View(product);
        }
    }
}
