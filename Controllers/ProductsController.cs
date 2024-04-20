using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PawsCare.Data;
using PawsCare.Interfaces;
using PawsCare.Migrations;
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

        public PartialViewResult GetMenuList()
        {
            var userId = GetUserId();

            var cart = _cartService.GetCartForUser(userId);
            var cartCount = _cartService.GetCartTotal(userId);

            var viewModel = new CartViewModel
            {
                Cart = cart,
                CartCount = cartCount
            };
            return PartialView("_MenuPartial", viewModel);
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
            if (User?.Identity?.IsAuthenticated == false)
            {
                return Json(new { success = false, message = "Please Login to Add items to cart" });
            }
            var userId = GetUserId();

            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
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
        public int GetUserId()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(userId);
        }
        public IActionResult RemoveItemFromCart(int productId, int cartId)
        {
            var userId = GetUserId();
            var cartItem = _context.CartItems
                .SingleOrDefault(ci => ci.ProductId == productId && ci.CartId == cartId);

            if (cartItem == null)
            {
                return Json(new { success = false, message = "Cart item not found" });
            }

            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();

            return Json(new { success = true, message = "Product removed successfully!" });
        }
        public IActionResult Cart()
        {
            Cart cart = new Cart();

            if (User?.Identity?.IsAuthenticated == true)
            {
                var userId = GetUserId();
                cart = _cartService.GetCartForUser(userId);
            }

            return View("Cart", cart);
        }
        public IActionResult IncreaseCartQuantity(int productId)
        {
            var userId = GetUserId();

            var cartId = _context.Carts.Where(u => u.UserId == userId).Select(u => u.Id).FirstOrDefault();

            var cartItem = _context.CartItems.FirstOrDefault(p => p.ProductId == productId && p.CartId == cartId);
            if (cartItem == null)
            {
                return Json(new { success = false, message = "Cart item not found" });
            }
            else
            {
                cartItem.Quantity++;
                _context.SaveChanges();
            }

            return Json(new { success = true, message = "Quantity increased!" });
        }
        public IActionResult DecreaseCartQuantity(int productId)
        {
            var userId = GetUserId();

            var cartId = _context.Carts.Where(u => u.UserId == userId).Select(u => u.Id).FirstOrDefault();

            var cartItem = _context.CartItems.FirstOrDefault(p => p.ProductId == productId && p.CartId == cartId);
            if (cartItem == null)
            {
                return Json(new { success = false, message = "Cart item not found" });
            }

            cartItem.Quantity--;

            if (cartItem.Quantity == 0)
            {
                RemoveItemFromCart(productId, cartId);
            }

            _context.SaveChanges();

            return Json(new { success = true, message = "Quantity decreased!" });
        }
    }
}
