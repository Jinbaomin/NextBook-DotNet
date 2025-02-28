using WebApplication1.Models;
using WebApplication1.Repository.IRepository;
using WebApplication1.Data;

namespace WebApplication1.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUser
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ApplicationUser obj)
        {
            _db.ApplicationUsers.Update(obj);
        }
    }
}
