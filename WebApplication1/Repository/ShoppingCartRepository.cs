using WebApplication1.Models;
using WebApplication1.Repository.IRepository;
using WebApplication1.Data;

namespace WebApplication1.Repository
{
    public class ShoppingCartRepository : Repository<Models.ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ShoppingCart obj)
        {
            _db.ShoppingCarts.Update(obj);
        }
    }
}
