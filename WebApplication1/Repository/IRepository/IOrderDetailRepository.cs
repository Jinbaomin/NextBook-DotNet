using WebApplication1.Data;
using WebApplication1.Models;   

namespace WebApplication1.Repository.IRepository
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail obj);
    }
}
