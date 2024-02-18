using Contracts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Contracts.Common.Interfaces
{

    public interface IRepositoryBaseAsync<T, TK, TContext> : IRepositoryQueryBaseAsync<T, TK, TContext>
        where T : EntityBase<TK>
        where TContext : DbContext
    {
        Task<TK> CreateAsync(T entity);
        Task<IList<TK>> CreateListAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateListAsync(IEnumerable<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteListAsync(IEnumerable<T> entities);
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task EndTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
