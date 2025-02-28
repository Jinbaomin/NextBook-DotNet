using WebApplication1.Models;

namespace WebApplication1.Repository.IRepository
{
    public interface IApplicationUser : IRepository<ApplicationUser>
    {
        void Update(ApplicationUser obj);
    }
}
