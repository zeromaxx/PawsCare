using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PawsCare.Data;
using PawsCare.Interfaces;
using System.Security.Claims;

namespace PawsCare.Models
{
	public class MenuViewComponent : ViewComponent
	{
		private readonly ICartService _cartService;
        private readonly AppDbContext _context;
        public MenuViewComponent(ICartService cartService, AppDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var cart = new Cart();

            if (UserClaimsPrincipal.Identity.IsAuthenticated)
            {
                string userId = UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userId, out int userIdAsInt))
                {
                    cart = _cartService.GetCartForUser(userIdAsInt) ?? new Cart();
                }
            }

            return View(cart);
        }
    }
}
