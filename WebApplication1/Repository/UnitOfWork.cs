using WebApplication1.Data;
using WebApplication1.Repository.IRepository;

namespace WebApplication1.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository categoryRepository { get; private set; }

        public IProductRepository productRepository { get; private set; }

        public ICompanyRepository companyRepository { get; private set; }
        public IShoppingCartRepository shoppingCartRepository { get; private set; }

        public IOrderDetailRepository orderDetailRepository { get; private set; }

        public IOrderHeaderRepository orderHeaderRepository { get; private set; }

        public IApplicationUser applicationUserRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            categoryRepository = new CategoryRepository(_db);
            productRepository = new ProductRepository(_db);
            companyRepository = new CompanyRepository(_db);
            shoppingCartRepository = new ShoppingCartRepository(_db);
            orderDetailRepository = new OrderDetailRepository(_db);
            orderHeaderRepository = new OrderHeaderRepository(_db);
            applicationUserRepository = new ApplicationUserRepository(_db);
        }
        
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
