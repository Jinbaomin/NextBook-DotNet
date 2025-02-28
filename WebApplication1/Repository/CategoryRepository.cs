using System.Linq.Expressions;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repository.IRepository;

namespace WebApplication1.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //public void Save()
        //{
        //    _db.SaveChanges();
        //    //throw new NotImplementedException();
        //}

        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
            //throw new NotImplementedException();
        }
    }
}
