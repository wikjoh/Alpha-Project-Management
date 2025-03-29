using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using Data.Contexts;
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
    public virtual async Task AddAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            await _dbSet.AddAsync(entity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating {nameof(TEntity)} entity. {ex.Message}");
            throw;
        }
    }


    // READ
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeExpression = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includeExpression != null)
            query = includeExpression(query);

        var entities = await query.ToListAsync();
        return entities;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllWhereAsync(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeExpression = null)
    {
        ArgumentNullException.ThrowIfNull(expression);

        IQueryable<TEntity> query = _dbSet;

        if (includeExpression != null)
            query = includeExpression(query);

        var entities = await query
            .Where(expression)
            .ToListAsync();

        return entities;
    }

    public virtual async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeExpression = null)
    {
        ArgumentNullException.ThrowIfNull(expression);

        IQueryable<TEntity> query = _dbSet;

        if (includeExpression != null)
            query = includeExpression(query);

        var entity = await query.FirstOrDefaultAsync(expression);

        return entity;
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        try
        {
            return await _dbSet.AnyAsync(expression);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error checking if {nameof(TEntity)} exists. {ex.Message}");
            throw;
        }
    }


    // UPDATE
    public virtual void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            _dbSet.Update(entity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating {nameof(TEntity)} entity. {ex.Message}");
            throw;
        }
    }


    // DELETE
    public virtual void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            _dbSet.Remove(entity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting {nameof(TEntity)} entity. {ex.Message}");
            throw;
        }
    }


    // SAVE CHANGES
    public virtual async Task<int?> SaveAsync()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Debug.WriteLine($"Error saving changes. {ex.Message}");
            throw;
        }
    }
}
