using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqKit;
using WebShop.Identity.Context;
using WebShop.Identity.Interfaces;

namespace WebShop.Identity.Repository
{
    public class IdentityGlobalRepository<T> : IIdentityGlobalRepository<T> where T : class
    {
        protected IDbSet<T> _dbSet;
        protected DbContextIdentity _context;

        public IdentityGlobalRepository(DbContextIdentity context)
        {
            _dbSet = context.Set<T>();
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public Task<T> GetByQueryAsync(Expression<Func<T, bool>> expr, params Expression<Func<T, object>>[] include)
        {
            IQueryable<T> query = _dbSet;
            var item = include.Aggregate(query, (a, i) => a.Include(i)).AsExpandable().Where(expr);
            return item.FirstOrDefaultAsync();
        }

        public Task<TResult> QueryProjectionAsync<TResult>(Expression<Func<T, bool>> expr, Expression<Func<T, TResult>> @select, params Expression<Func<T, object>>[] include)
        {
            IQueryable<T> query = _dbSet;
            var item = include.Aggregate(query, (a, i) => a.Include(i)).AsExpandable().Where(expr).
                Select(select);
            return item.FirstOrDefaultAsync();
        }

        public virtual T Add(T item)
        {
            return default(T);
        }

        public virtual void Delete(T item)
        {
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Update(T item)
        {
        }
       
        public TResult DisabledProxy<TResult>() where TResult : IIdentityGlobalRepository<T>
        {
            _context.Configuration.ProxyCreationEnabled = false;
            return (TResult)(this as IIdentityGlobalRepository<T>);
        }

        public TResult EnableProxy<TResult>() where TResult : IIdentityGlobalRepository<T>
        {
            _context.Configuration.ProxyCreationEnabled = true;
            return (TResult)(this as IIdentityGlobalRepository<T>); ;
        }

        public int GetCount()
        {
            return 0;
        }
    }
}