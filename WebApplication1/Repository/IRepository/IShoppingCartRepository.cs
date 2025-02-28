using WebApplication1.Models;

namespace WebApplication1.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<Models.ShoppingCart>
    {
        void Update(Models.ShoppingCart obj);
    }
}
