using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Repository.IRepository;
using System.Linq.Expressions;

namespace WebApplication1.Repository;

public class Repository<T> : IRepository<T> where T : class
{

    private readonly ApplicationDbContext _db;

    internal DbSet<T> dbSet;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
        this.dbSet = _db.Set<T>();
        // _db.Categories == dbSet;
        // _db.Categories.Add(obj);

    }
    public void Add(T entity)
    {
        dbSet.Add(entity);
        //throw new NotImplementedException();
    }

    public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        if(filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }
            return query.FirstOrDefault();
        //throw new NotImplementedException();
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        if(filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }
            return query.ToList();
        //throw new NotImplementedException();
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
        //throw new NotImplementedException();
    }

    public void DeleteRange(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
        //throw new NotImplementedException();
    }

}
