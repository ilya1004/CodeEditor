using CodeEditor.Core.Entities;

namespace CodeEditor.Core.Abstractions.Data;

public interface IUnitOfWork
{
    IRepository<ErrorLog> ErrorLogRepository { get; }
    Task SaveAllAsync(CancellationToken cancellationToken = default);
}