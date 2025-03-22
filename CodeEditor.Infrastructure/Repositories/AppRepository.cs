using System.Linq.Expressions;
using CodeEditor.Core.Abstractions.Data;
using CodeEditor.Core.Entities;
using CodeEditor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeEditor.Infrastructure.Repositories;

public class AppRepository<TEntity>(ApplicationDbContext context) : IRepository<TEntity> where TEntity : Entity
{
    protected readonly DbSet<TEntity> _entities = context.Set<TEntity>();

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _entities.AddAsync(entity, cancellationToken);
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _entities.Remove(entity);

        return Task.CompletedTask;
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
    {
        return await _entities.AsNoTracking().FirstOrDefaultAsync(filter, cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _entities.AsQueryable().AsNoTracking();

        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<List<TEntity>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return await _entities.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<List<TEntity>> PaginatedListAllAsync(int offset, int limit, CancellationToken cancellationToken = default)
    {
        return await _entities
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? filter,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _entities.AsQueryable().AsNoTracking();

        if (filter != null) query = query.Where(filter);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<TEntity>> PaginatedListAsync(Expression<Func<TEntity, bool>>? filter, int offset, int limit,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _entities.AsQueryable().AsNoTracking();

        if (filter != null) query = query.Where(filter);

        return await query
            .OrderBy(x => x.Id)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _entities.Update(entity);

        return Task.CompletedTask;
    }

    public async Task<int> CountAllAsync(CancellationToken cancellationToken = default)
    {
        return await _entities.CountAsync(cancellationToken);
    }
}