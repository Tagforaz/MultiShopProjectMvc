

namespace MultiShopProjectMVC.Models
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image {  get; set; }
        public string Description { get; set; }



        //relational
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
