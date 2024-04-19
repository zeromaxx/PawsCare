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
        public MenuViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }
		public IViewComponentResult Invoke()
		{
			var viewModel = new CartViewModel();

			if (UserClaimsPrincipal.Identity.IsAuthenticated)
			{
				string userId = UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				if (int.TryParse(userId, out int userIdAsInt))
				{
					var cart = _cartService.GetCartForUser(userIdAsInt);
					if (cart != null)
					{
						viewModel.Cart = cart;
						viewModel.CartCount = _cartService.GetCartTotal(userIdAsInt); // Assuming GetCartTotal calculates total price
					}
				}
			}

			return View("_MenuPartial", viewModel); // Make sure to pass the correct view name and viewModel
		}
	}
}
