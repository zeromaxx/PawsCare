using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PawsCare.Data;
using PawsCare.Interfaces;
using PawsCare.Models;
using PawsCare.Models.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace PawsCare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
		private readonly ICartService _cartService;

		public HomeController(ILogger<HomeController> logger, AppDbContext context, ICartService cartService)
        {
            _logger = logger;
            _context = context;
            _cartService = cartService;
        }

        public IActionResult Index()
        {
			List<int> productIdsInCart = new List<int>();

			if (User?.Identity?.IsAuthenticated == true)
			{
				var userId = GetUserId();
				var cartItems = _context.Carts
						.Include(c => c.CartItems)
						.Where(c => c.UserId == userId)
						.SelectMany(c => c.CartItems)
						.ToList();

				productIdsInCart = cartItems.Select(ci => ci.ProductId).ToList();
			}
            var products = _context.Products.ToList();

			var productViewModels = products.Select(p => new ProductsHomeViewModel
			{
				Product = p,
				HasBeenAddedToCart = productIdsInCart.Contains(p.Id)
			}).ToList();

			return View(productViewModels);
        }
        public IActionResult Contacts()
        {
            return View();
        }
		public IActionResult Checkout()
		{
			var userId = GetUserId();

			var cart = _cartService.GetCartForUser(userId);
			var cartCount = _cartService.GetCartTotal(userId);

			var viewModel = new CheckoutViewModel
			{
				Cart = cart,
				CartCount = cartCount
			};
			return View(viewModel);
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
		public int GetUserId()
		{
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			return int.Parse(userId);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}