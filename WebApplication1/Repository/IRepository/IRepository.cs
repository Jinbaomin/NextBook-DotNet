using System.Drawing;
using System.Linq.Expressions;

namespace WebApplication1.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        // T - Category
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties= null);
        //T GetFirstOrDefault();
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
        void Add(T entity);
        //void Update(T entity);
        void Remove(T entity);
        void DeleteRange(IEnumerable<T> entity);
    }
}
