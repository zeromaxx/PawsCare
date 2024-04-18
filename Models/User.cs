using System.ComponentModel.DataAnnotations;

namespace PawsCare.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "user";
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
		public List<Cart> Carts { get; set; } = new List<Cart>();
       
	}
}
