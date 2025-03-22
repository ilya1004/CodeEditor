using System.Linq.Expressions;
using CodeEditor.Core.Entities;

namespace CodeEditor.Core.Abstractions.Data;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task<List<TEntity>> ListAllAsync(CancellationToken cancellationToken = default);
    Task<List<TEntity>> PaginatedListAllAsync(int offset, int limit, CancellationToken cancellationToken = default);
    Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? filter, CancellationToken cancellationToken = default);
    Task<List<TEntity>> PaginatedListAsync(Expression<Func<TEntity, bool>>? filter, int offset, int limit,
        CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
    Task<int> CountAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}