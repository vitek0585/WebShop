using System.Data;
using System.Data.Entity;
using WebShop.Domain.Interfaces;
using WebShop.Identity.Interfaces;

namespace WebShop.Domain.Services.UOF
{
    public class UnitOfWorkIdentity : IUnitOfWorkIdentity
    {
        private DbContext _context;
        private DbContextTransaction _transaction;
        public UnitOfWorkIdentity(DbContext context)
        {
            _context = context;
        }

        public void StartTransaction(IsolationLevel level)
        {
            _transaction = _context.Database.BeginTransaction(level);
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();
        }
    }
}