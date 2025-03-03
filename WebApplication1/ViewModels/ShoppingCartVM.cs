
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> shoppingCartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
        //public double OrderTotal { get; set; }
    }
}
