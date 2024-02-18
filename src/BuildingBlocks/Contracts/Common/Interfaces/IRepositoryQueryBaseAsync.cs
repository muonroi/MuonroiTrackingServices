using System.Linq.Expressions;
using Contracts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Contracts.Common.Interfaces
{
    public interface IRepositoryQueryBaseAsync<T, in TK, TContext> where T : EntityBase<TK>
        where TContext : DbContext
    {
        IQueryable<T> FindAll(bool trackChanges = false);
        IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
            params Expression<Func<T, object>>[] includeProperties);

        Task<T?> GetByIdAsync(TK id);

        Task<T?> GetByIdAsync(TK id, params Expression<Func<T, object>>[] includeProperties);
    }

}
