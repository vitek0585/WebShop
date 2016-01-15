using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebShop.Identity.Interfaces
{
    public interface IIdentityGlobalRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<T> GetByQueryAsync
            (Expression<Func<T, bool>> expr, params Expression<Func<T, object>>[] include);
        Task<TResult> QueryProjectionAsync<TResult>(Expression<Func<T, bool>> expr, Expression<Func<T, TResult>> select,
            params Expression<Func<T, object>>[] include);

        T Add(T item);
        void Delete(T item);
        Task<int> SaveAsync();
        void Update(T item);
        TResult DisabledProxy<TResult>() where TResult : IIdentityGlobalRepository<T>;
        TResult EnableProxy<TResult>() where TResult : IIdentityGlobalRepository<T>;
        int GetCount();
    }
}