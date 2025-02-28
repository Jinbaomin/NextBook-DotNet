using WebApplication1.Data;
using WebApplication1.Models;   

namespace WebApplication1.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
    }
}
