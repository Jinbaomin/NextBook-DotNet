using WebApplication1.Data;

namespace WebApplication1.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository categoryRepository { get; }
        IProductRepository productRepository { get; }
        ICompanyRepository companyRepository { get; }
        IShoppingCartRepository shoppingCartRepository { get; }
        IOrderDetailRepository orderDetailRepository { get; }
        IOrderHeaderRepository orderHeaderRepository { get; }
        IApplicationUser applicationUserRepository { get; }
        void Save();
    }
}
