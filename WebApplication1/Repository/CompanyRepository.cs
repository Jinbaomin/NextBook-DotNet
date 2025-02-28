using System.Linq.Expressions;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repository.IRepository;

namespace WebApplication1.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company obj)
        {
            _db.Companies.Update(obj);
        }
    }
}
