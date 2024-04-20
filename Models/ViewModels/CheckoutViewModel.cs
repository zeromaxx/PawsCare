using System.ComponentModel.DataAnnotations;

namespace PawsCare.Models.ViewModels
{
    public class CheckoutViewModel
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Country { get; set; }
        [Required]
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public Cart? Cart { get; set; }
        public decimal? CartCount { get; set; }
    }
}
