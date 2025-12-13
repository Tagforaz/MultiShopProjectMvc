namespace MultiShopProjectMVC.ViewModels
{
    public class UpdateCategoryVM
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
