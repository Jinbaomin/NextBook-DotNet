using System.Linq.Expressions;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repository.IRepository;

namespace WebApplication1.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //public void Save()
        //{
        //    _db.SaveChanges();
        //    //throw new NotImplementedException();
        //}

        public void Update(Product obj)
        {
            _db.Products.Update(obj);
            //throw new NotImplementedException();
        }
    }
}
