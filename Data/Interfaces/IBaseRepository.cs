using Data.Models;
using System.Linq.Expressions;

namespace Data.Interfaces;
public interface IBaseRepository<TEntity, TMapTo>
{
    Task<RepositoryResult<bool?>> AddAsync(TEntity entity);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    RepositoryResult<bool?> Delete(TEntity entity);
    Task<RepositoryResult<bool?>> ExistsAsync(Expression<Func<TEntity, bool>> expression);
    Task<RepositoryResult<IEnumerable<TMapTo>>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes);
    Task<RepositoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector, bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes);
    Task<RepositoryResult<IEnumerable<TEntity>>> GetAllEntitiesByQueryAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryBuilder = null);

    //Task<RepositoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector, bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>[]? wheres = null, params Expression<Func<TEntity, object>>[] includes);
    Task<RepositoryResult<TMapTo>> GetAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);
    Task<RepositoryResult<TEntity>> GetEntityAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);
    Task<RepositoryResult<TEntity>> GetEntityByQueryAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryBuilder = null);
    Task RollbackTransactionAsync();
    Task<RepositoryResult<bool?>> SaveAsync();
    RepositoryResult<bool?> Update(TEntity entity);
}