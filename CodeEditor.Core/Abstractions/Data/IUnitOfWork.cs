using CodeEditor.Core.Entities;

namespace CodeEditor.Core.Abstractions.Data;

public interface IUnitOfWork
{
    IRepository<ErrorLog> ErrorLogsRepository { get; }
    IRepository<Language> LanguagesRepository { get; }
    Task SaveAllAsync(CancellationToken cancellationToken = default);
}