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
    Task<RepositoryResult<IEnumerable<TMapTo>>> GetAllAsync();
    Task<RepositoryResult<TMapTo>> GetAsync(Expression<Func<TEntity, bool>> expression);
    Task RollbackTransactionAsync();
    Task<RepositoryResult<bool?>> SaveAsync();
    RepositoryResult<bool?> Update(TEntity entity);
}