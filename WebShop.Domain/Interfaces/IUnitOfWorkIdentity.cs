using System.Data;

namespace WebShop.Domain.Interfaces
{
    public interface IUnitOfWorkIdentity
    {
        void StartTransaction(IsolationLevel level = IsolationLevel.RepeatableRead);
        void Commit();
        void Rollback();
        void Save();
    }
}