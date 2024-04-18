using Microsoft.AspNetCore.Mvc;
using PawsCare.Data;
using PawsCare.Interfaces;
using PawsCare.Models;
using PawsCare.Models.ViewModels;
using System.Security.Claims;

namespace PawsCare.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICartService _cartService;
        public ProductsController(AppDbContext context, ICartService cartService) 
        {
            _cartService = cartService;
            _context = context;
        }

		public PartialViewResult getMenuList()
		{
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int parsedUserId = int.Parse(userId);
            var cart = _cartService.GetCartForUser(parsedUserId);
            return PartialView("_MenuPartial", cart);
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
        [HttpPost]
        public IActionResult AddToCart([FromBody] ProductCartViewModel viewModel)
        {
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			int parsedUserId = int.Parse(userId);

			var cart = _context.Carts.FirstOrDefault(c => c.UserId == parsedUserId);

			if (cart == null)
			{
				cart = new Cart
				{
					UserId = parsedUserId
				};
				_context.Carts.Add(cart);
				_context.SaveChanges();
			};

			var cartItem = _context.CartItems.FirstOrDefault(ci => ci.CartId == cart.Id && ci.ProductId == viewModel.ProductId);

			if (cartItem != null)
			{
				cartItem.Quantity += viewModel.Quantity;
			}
			else
			{
				cartItem = new CartItem
				{
					CartId = cart.Id,
					ProductId = viewModel.ProductId,
					Quantity = viewModel.Quantity
				};
				_context.CartItems.Add(cartItem);
			}

			_context.SaveChanges();

			return Json(new { success = true, message = "Product saved successfully!" });
		}
    }
}
