using MultiShopProjectMVC.Models;

namespace MultiShopProjectMVC.ViewModels
{
    public class GetCategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        //relational
        public List<Product>? Products { get; set; }
    }
}
