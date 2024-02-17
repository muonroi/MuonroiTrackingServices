using System.Linq.Expressions;
using Contracts.Common.Interfaces;
using Contracts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Common
{
    public class RepositoryBaseAsync<T, TK, TContext> : IRepositoryBaseAsync<T, TK, TContext> where T : EntityBase<TK> where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly IUnitOfWork<TContext> _unitOfWork;

        public RepositoryBaseAsync(TContext context, IUnitOfWork<TContext> unitOfWork)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IQueryable<T> FindAll(bool trackChanges = false) =>
            !trackChanges ? _context.Set<T>().AsNoTracking() : _context.Set<T>();

        public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
        {
            var items = FindAll(trackChanges);
            items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
            return items;
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false) =>
        !trackChanges ? _context.Set<T>().Where(expression).AsNoTracking()
            : _context.Set<T>().Where(expression);

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
        {
            var items = FindByCondition(expression, trackChanges);
            items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
            return items;
        }

        public async Task<T?> GetByIdAsync(TK id) => await FindByCondition(x => x.Id != null && x.Id.Equals(id)).FirstOrDefaultAsync();

        public async Task<T?> GetByIdAsync(TK id, params Expression<Func<T, object>>[] includeProperties) =>
            await FindByCondition(x => x.Id != null && x.Id.Equals(id), trackChanges: false, includeProperties)
                .FirstOrDefaultAsync();

        public async Task<TK> CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity.Id;
        }

        public async Task<IList<TK>> CreateListAsync(IEnumerable<T> entities)
        {
            var entityBases = entities as T[] ?? entities.ToArray();
            await _context.Set<T>().AddRangeAsync(entityBases);
            return entityBases.Select(x => x.Id).ToList();
        }

        public Task UpdateAsync(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Unchanged) return Task.CompletedTask;
            var exist = _context.Set<T>().Find(entity.Id);
            _context.Entry(exist).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }

        public Task UpdateListAsync(IEnumerable<T> entities) => _context.Set<T>().AddRangeAsync(entities);

        public Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteListAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public Task<int> SaveChangesAsync() => _unitOfWork.CommitAsync();

        public Task<IDbContextTransaction> BeginTransactionAsync() => _context.Database.BeginTransactionAsync();

        public async Task EndTransactionAsync()
        {
            await SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }

        public Task RollbackTransactionAsync() => _context.Database.RollbackTransactionAsync();
    }
}
