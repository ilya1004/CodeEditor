using CodeEditor.Core.Abstractions.Data;
using CodeEditor.Core.Models;
using CodeEditor.Infrastructure.Data;

namespace CodeEditor.Infrastructure.Repositories;

public class AppUnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly Lazy<IRepository<ErrorLog>> _errorLogRepository = new(() => new AppRepository<ErrorLog>(context));
    
    public IRepository<ErrorLog> ErrorLogRepository => _errorLogRepository.Value;

    public async Task SaveAllAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}