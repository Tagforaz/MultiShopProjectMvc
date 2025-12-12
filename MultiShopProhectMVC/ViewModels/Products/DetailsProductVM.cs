using MultiShopProjectMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace MultiShopProjectMVC.ViewModels
{
    public class DetailsProductVM
    {
        public string Name { get; set; }
        [Required]
        [Range(0.01D, (double)decimal.MaxValue, ErrorMessage = "The value  must be a positive ")]
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public IFormFile? Photo { get; set; }
        [Required]
        public int? CategoryId { get; set; }
        public List<Category>? Categories { get; set; }
        public string Image { get; set; }
    }
}
