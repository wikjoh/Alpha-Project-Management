using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using Data.Contexts;
using Data.Models;
using Data.Interfaces;
using Domain.Extensions;

namespace Data.Repositories;

public abstract class BaseRepository<TEntity, TMapTo>(AppDbContext context) : IBaseRepository<TEntity, TMapTo> where TEntity : class
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
    public virtual async Task<RepositoryResult<IEnumerable<TMapTo>>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (where != null)
            query = query.Where(where);

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        if (sortBy != null)
            query = orderByDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);

        var entities = await query.ToListAsync();
        var result = entities.Select(entity => entity.MapTo<TMapTo>());
        return RepositoryResult<IEnumerable<TMapTo>>.Ok(result);
    }

    public virtual async Task<RepositoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector, bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (where != null)
            query = query.Where(where);

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        if (sortBy != null)
            query = orderByDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);

        var entities = await query.Select(selector).ToListAsync();
        var result = entities.Select(entity => entity!.MapTo<TSelect>());
        return RepositoryResult<IEnumerable<TSelect>>.Ok(result);
    }

    public virtual async Task<RepositoryResult<TMapTo>> GetAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);


        var entity = await query.FirstOrDefaultAsync(where);
        if (entity == null)
            return RepositoryResult<TMapTo>.NotFound("Entity not found.");

        var result = entity.MapTo<TMapTo>();
        return RepositoryResult<TMapTo>.Ok(result);
    }

    public virtual async Task<RepositoryResult<TEntity>> GetEntityAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);


        var entity = await query.FirstOrDefaultAsync(where);
        if (entity == null)
            return RepositoryResult<TEntity>.NotFound("Entity not found.");

        return RepositoryResult<TEntity>.Ok(entity);
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
