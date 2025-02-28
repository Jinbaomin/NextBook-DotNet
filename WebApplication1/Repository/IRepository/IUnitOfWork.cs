using WebApplication1.Data;

namespace WebApplication1.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository categoryRepository { get; }
        IProductRepository productRepository { get; }
        ICompanyRepository companyRepository { get; }
        IShoppingCartRepository shoppingCartRepository { get; }
        void Save();
    }
}
