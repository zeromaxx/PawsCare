using Microsoft.EntityFrameworkCore;
using PawsCare.Data;
using PawsCare.Interfaces;

namespace PawsCare.Models
{
	public class CartService:ICartService
	{
		private readonly AppDbContext _context;
		public CartService(AppDbContext context) 
		{  
			_context = context; 
		}

		public Cart GetCartForUser(int userId)
		{
			return _context.Carts
					 .Include(c => c.CartItems)
					 .ThenInclude(ci => ci.Product)
					 .FirstOrDefault(c => c.UserId == userId);
		}
		public decimal GetCartTotal(int userId)
		{
			var cart = GetCartForUser(userId);
			if (cart?.CartItems == null || !cart.CartItems.Any())
			{
				return 0m;
			}

			return cart.CartItems.Sum(item => item.Quantity * item.Product.Price);
		}

	}
}
