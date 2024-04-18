using PawsCare.Models;

namespace PawsCare.Interfaces
{
	public interface ICartService
	{
		Cart GetCartForUser(int userId);
	}
}
