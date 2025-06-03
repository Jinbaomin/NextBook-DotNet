using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class ProductSearch
    {
        public List<Product> listProduct { get; set; }
        public String searchTerm { get; set; }
    }
}
