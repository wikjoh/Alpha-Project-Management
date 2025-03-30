using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using Data.Contexts;
using Data.Models;
using Data.Interfaces;

namespace Data.Repositories;

public abstract class BaseRepository<TEntity>(AppDbContext context) : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext _context = context;
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    private IDbContextTransaction _transaction = null!;


    #region Transaction Management
    public virtual async Task BeginTransactionAsync()
    {
        _transaction ??= await _context.Database.BeginTransactionAsync();
    }

    public virtual async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }

    public virtual async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }
    #endregion


    // CREATE
    public virtual async Task<RepositoryResult<bool?>> AddAsync(TEntity entity)
    {
        if (entity == null)
            return RepositoryResult<bool?>.BadRequest("Entity cannot be null.");

        try
        {
            await _dbSet.AddAsync(entity);
            return RepositoryResult<bool?>.Ok();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return RepositoryResult<bool?>.InternalServerErrror(ex.Message);
        }
    }


    // READ
    public virtual async Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync()
    {
        var entities = await _dbSet.ToListAsync();
        return RepositoryResult<IEnumerable<TEntity>>.Ok(entities);
    }

    public virtual async Task<RepositoryResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression)
    {
        if (expression == null)
            return RepositoryResult<TEntity>.BadRequest("Expression cannot be null.");

        var entity = await _dbSet.FirstOrDefaultAsync(expression);
        return entity == null
            ? RepositoryResult<TEntity>.NotFound("Entity not found.")
            : RepositoryResult<TEntity>.Ok(entity);
    }

    public virtual async Task<RepositoryResult<bool?>> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        if (expression == null)
            return RepositoryResult<bool?>.BadRequest("Expression cannot be null.");

        var exists = await _dbSet.AnyAsync(expression);
        return !exists
            ? RepositoryResult<bool?>.NotFound("Entity not found.")
            : RepositoryResult<bool?>.Ok(exists);
    }


    // UPDATE
    public virtual RepositoryResult<bool?> Update(TEntity entity)
    {
        if (entity == null)
            return RepositoryResult<bool?>.BadRequest("Entity cannot be null.");

        try
        {
            _dbSet.Update(entity);
            return RepositoryResult<bool?>.Ok();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return RepositoryResult<bool?>.InternalServerErrror(ex.Message);
        }
    }


    // DELETE
    public virtual RepositoryResult<bool?> Delete(TEntity entity)
    {
        if (entity == null)
            return RepositoryResult<bool?>.BadRequest("Entity cannot be null.");

        try
        {
            _dbSet.Remove(entity);
            return RepositoryResult<bool?>.Ok();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return RepositoryResult<bool?>.InternalServerErrror(ex.Message);
        }
    }


    // SAVE CHANGES
    public virtual async Task<RepositoryResult<bool?>> SaveAsync()
    {
        try
        {
            var result = await _context.SaveChangesAsync() > 0;
            return RepositoryResult<bool?>.Ok();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return RepositoryResult<bool?>.InternalServerErrror(ex.Message);
        }
    }
}
