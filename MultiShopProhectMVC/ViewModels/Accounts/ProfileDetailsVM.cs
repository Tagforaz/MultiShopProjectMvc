using MultiShopProjectMVC.Utilities;

namespace MultiShopProjectMVC.ViewModels
{
    public class ProfileDetailsVM
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly Birthday { get; set; }
        public string Image { get; set; }
        public Gender Gender { get; set; }
    }
}
